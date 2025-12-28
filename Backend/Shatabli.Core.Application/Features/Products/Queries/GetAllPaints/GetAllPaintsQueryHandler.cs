using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shatabli.Core.Application.Features.Products.Queries.GetAllPaints.GetAllPaintsResponse;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllPaints
{
    public class GetAllPaintsQueryHandler : IRequestHandler<GetAllPaintsQuery, GetAllPaintsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPaintsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<GetAllPaintsResponse> IRequestHandler<GetAllPaintsQuery, GetAllPaintsResponse>.Handle(GetAllPaintsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Products
                 .Where(p => p.Category == ProductCategory.WallPaints)
                 .ProjectTo<PaintsDTO>(_mapper.ConfigurationProvider).ToListAsync();

            GetAllPaintsResponse response = new();
            response.paintsList = result;

            return response;
        }
    }
}

