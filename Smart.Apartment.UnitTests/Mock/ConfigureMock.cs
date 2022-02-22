using Moq;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.Application.Features.Searches.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Apartment.UnitTests.Mock
{
    public class ConfigureMock
    {
        public static Mock<ISearchService> GetSearchMarketListServiceMock()
        {
            var marketList = new List<MarketListVm>
            {
                new MarketListVm
                {
                    Market = "Houston"
                },
                new MarketListVm
                {
                    Market = "Atlanta"
                },
                new MarketListVm
                {
                    Market = "Austin"
                },
                 new MarketListVm
                {
                    Market = "Orange County"
                }
            };

            //var searchResult = new List<SearchListContentVm>
            //{
            //    new SearchListContentVm
            //    {
            //        IsManagement = true,
            //        Name = "Houston Rand Ltd",
            //        State = "TX",
            //        Market = "Houston"
            //    },
            //    new SearchListContentVm 
            //    { 
            //        IsManagement = false,
            //        Name = "Emerald Pointe",
            //        State = "TX",
            //        Market = "Houston"
            //    }
            //};
  
            var searchService = new Mock<ISearchService>();
            searchService.Setup(repo => repo.GetMarketList()).ReturnsAsync(marketList);

            //searchService.Setup(repo => repo.SearchSmartAparment(It.IsAny<string>(), It.IsAny<string>())).Returns(
            //    (SearchListContentVm result) =>
            //    {
            //        searchResult.Add(result);
            //        return result;
            //    });

            return searchService;
        }

        public static Mock<ISearchService> GetSearchSearchServiceMock()
        {

            var searchResult = new List<SearchListContentVm>
            {
                new SearchListContentVm
                {
                    IsManagement = true,
                    Name = "Houston Rand Ltd",
                    State = "TX",
                    Market = "Houston"
                },
                new SearchListContentVm
                {
                    IsManagement = false,
                    Name = "Emerald Pointe",
                    State = "TX",
                    Market = "Houston"
                }
            };

            var searchService = new Mock<ISearchService>();

             searchService.Setup(repo => repo.SearchSmartAparment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                (SearchListContentVm result, string o) =>
                {
                    searchResult.Add(result);
                    return searchResult;
                });

            return searchService;
        }


    }
}
