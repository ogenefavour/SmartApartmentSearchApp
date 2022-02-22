using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.Application.Features.Searches.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Apartment.Application.Contracts.Infrastructure
{
    public interface ISearchService
    {
        Task<List<MarketListVm>> GetMarketList();
        Task<List<SearchListContentVm>> SearchSmartAparment(string searchPhrase, string market);
    }
}
