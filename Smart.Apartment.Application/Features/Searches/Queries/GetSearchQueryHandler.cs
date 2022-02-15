using MediatR;
using Smart.Apartment.Application.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smart.Apartment.Application.Features.Searches.Queries
{
    public class GetSearchQuery : IRequest<ICollection<SearchListContentVm>>
    {
        public string searchPhrase { get; set; }

        public string Market { get; set; }
    }
    public class GetSearchQueryHandler : IRequestHandler<GetSearchQuery, ICollection<SearchListContentVm>>
    {
        private readonly ISearchService _searchService;
        public GetSearchQueryHandler(ISearchService searchService)
        {
            _searchService = searchService;
        }
        public async Task<ICollection<SearchListContentVm>> Handle(GetSearchQuery request, CancellationToken cancellationToken)
        {
            var smartApartment = await _searchService.SearchSmartAparment(request.searchPhrase, request.Market);
            return smartApartment;
        }
    }
}
