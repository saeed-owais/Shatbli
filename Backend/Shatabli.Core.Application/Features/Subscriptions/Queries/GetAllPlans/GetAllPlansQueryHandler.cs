using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, Result<List<GetAllPlansQueryResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public const string CacheKey = "AllPlans";
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(30);

        public GetAllPlansQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPlansQueryResponse>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            // Check cache
            if (_cache.TryGetValue(CacheKey, out List<GetAllPlansQueryResponse> cachedPlans))
            {
                return Result<List<GetAllPlansQueryResponse>>.Success(cachedPlans, "Plans retrieved from cache successfully");
            }

            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .ProjectTo<GetAllPlansQueryResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // Set cache
            _cache.Set(CacheKey, plans, CacheExpiration);

            return Result<List<GetAllPlansQueryResponse>>.Success(plans, "Plans retrieved successfully");
        }
    }
}