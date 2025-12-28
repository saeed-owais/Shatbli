using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Products.Commands.AddProductsFromExel
{
    public class AddProductsFromExelCommandHandler : IRequestHandler<AddProductsFromExelCommand, AddProductsFromExelResponse>
    {
        private readonly HttpClient httpClient;
        private readonly IStorageService _storageService;
        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public AddProductsFromExelCommandHandler(HttpClient httpClient, IStorageService storageService, IApplicationDbContext context, IMemoryCache cache)
        {
            this.httpClient = httpClient;
            _storageService = storageService;
            _context = context;
            _cache = cache;
        }
        async Task<AddProductsFromExelResponse> IRequestHandler<AddProductsFromExelCommand, AddProductsFromExelResponse>.Handle(AddProductsFromExelCommand request, CancellationToken cancellationToken)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var htttpClient = new HttpClient())
            using (var package = new ExcelPackage(request.stream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                int startRow = 2; // افتراض أن الصف الأول Headers
                int rowCount = worksheet.Dimension.Rows;

                for (int rowNum = startRow; rowNum <= rowCount; rowNum++)
                {
                    try
                    {
                        // 1. الاستخلاص (Extract): قراءة البيانات من الأعمدة
                        var productName = worksheet.Cells[rowNum, 2].Text;      // العمود A
                        var productSize = worksheet.Cells[rowNum, 3].Text; // العمود B
                        var productPrice = worksheet.Cells[rowNum, 4].Text;
                        var imageExternalUrl = worksheet.Cells[rowNum, 5].Text; // العمود C - رابط الصورة الخارجي

                        string finalImageUrl = null;
                        string productId = Guid.NewGuid().ToString();

                        if (!string.IsNullOrEmpty(imageExternalUrl))
                        {
                            // 2. التنزيل (Download):
                            var imageResponse = await httpClient.GetAsync(imageExternalUrl);
                            imageResponse.EnsureSuccessStatusCode();
                            // 3. الرفع (Upload):
                            using (var imageStream = await imageResponse.Content.ReadAsStreamAsync())
                            {
                                // توليد Public ID فريد للصورة (مثلاً اسم المجلد + UUID)
                                //string publicId = productName;  //$"{cloudinaryFolderName}/{Guid.NewGuid()}";
                                finalImageUrl = await _storageService.Upload(imageStream, productName, productId);
                            }
                        }

                        // 4. التحويل (Transform) و 5. التخزين (Load):
                        var product = new Product
                        {
                            Id = productId,
                            Name = productName,
                            //Size = productSize,
                            ImageUrl = finalImageUrl, // رابط Cloudinary النهائي
                            ImagePath = imageExternalUrl,                           // ... أي خصائص أخرى ...
                            Category = ProductCategory.FlooringCeramics
                        };

                        await _context.Products.AddAsync(product);
                    }
                    catch (Exception ex)
                    {
                        // يجب تسجيل الخطأ هنا لتحديد الصف الذي فشل
                        Console.WriteLine($"Failed to import row {rowNum}: {ex.Message}");
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                
                // Invalidate cache to ensure fresh data on next GetAllCeramics call
                _cache.Remove(GetAllCeramicsQueryHandler.CacheKey);
            }

            return new AddProductsFromExelResponse();
        }
    }
}
