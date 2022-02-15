using AutoMapper;
using MediatR;
using Smart.Apartment.Application.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smart.Apartment.Application.Features.Markets.Query.GetMarketList
{
    public class GetMarketListQuery : IRequest<List<MarketListVm>>
    {
    }
    public class GetMarketListQueryHandler : IRequestHandler<GetMarketListQuery, List<MarketListVm>>
    {
        private readonly ISearchService _searchService;
        //private readonly IMapper _mapper;
        public GetMarketListQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
            //_mapper = mapper;

        }
        public async Task<List<MarketListVm>> Handle(GetMarketListQuery request, CancellationToken cancellationToken)
        {
            var marketList = await _searchService.GetMarketList();
            return marketList;
        }
    }
}
