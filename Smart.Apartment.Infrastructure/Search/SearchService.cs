using Microsoft.Extensions.Options;
using Nest;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.Application.Features.Searches.Queries;
using Smart.Apartment.Application.Models;
using Smart.Apartment.Application.Models.AppSettings;
using Smart.Apartment.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Smart.Apartment.Infrastructure.Search
{
    public class SearchService : ISearchService
    {
        private readonly Filters _filter;
        private readonly IElasticClient _client;
        public SearchService(IOptions<Filters> filter, IElasticClient client)
        {
            _filter = filter.Value ?? throw new ArgumentNullException(nameof(filter));
            _client = client;
        }
        public async Task<List<MarketListVm>> GetMarketList()
        {
            var response = await _client.SearchAsync<ManagementModel> (s =>
                   s.Size(0)
                    .Aggregations(a =>a
                        .Terms("field", g => g
                            .Field(o => o.Market.Suffix("keyword")
                            ).Size(int.MaxValue)
                        )
                    ));
            var termsAggregate = response.Aggregations.Terms("field");
            var marketList = termsAggregate?.Buckets.Select(x => new MarketListVm()
            {
                Market = x.Key
            }).ToList();

            return marketList;
        }

        public async Task<List<SearchListContentVm>> SearchSmartAparment(string searchPhrase, string market)
        {
            searchPhrase = FilterHelper.FilterSearchString(searchPhrase, _filter.SearchFilter);
            market = HttpUtility.UrlDecode(market);

            var smartSearchResponse = new List<SearchListContentVm>();
            var propertyResponse = new List<SearchListContentVm>();
            var managementResponse = new List<SearchListContentVm>();

            if (!string.IsNullOrEmpty(market))
            {
                var searchManagementResponse = await _client.SearchAsync<ManagementModel>(a => a.Size(25)
                    .From(0)    
                    .Query(b => b
                        .Bool(c => c
                        .Must(o => o
                            .Match(f => f
                                .Field(fl => fl.Name)
                                .Query(searchPhrase)), m => m
                                    .Match(f => f
                                        .Field(f => f.Market)
                                            .Query(market))))));


                var searchPropertyResponse = await _client.SearchAsync<PropertiesModel>(s => s
                    .Size(25)
                    .From(0)
                    .Query(q => q.Bool(c => c
                        .Must(o => o
                        .MultiMatch(f => f
                            .Fields(f => f
                            .Field(f => f.FormerName)
                            .Field(f => f.State)
                            .Field(f => f.Market)
                            .Field(f => f.StreetAddress)).Query(searchPhrase)), p => p
                            .Match(m => m
                            .Field(f => f.Market)
                                .Query(market)
                             )
                        )
                    )));

                propertyResponse = searchPropertyResponse?.Documents.Select(res => new SearchListContentVm()
                {
                    Name = res.Name,
                    Market = res.Market,
                    State = res.State,
                    IsManagement = false
                }).ToList();

                managementResponse = searchManagementResponse?.Documents.Select(res => new SearchListContentVm()
                {
                    Name = res.Name,
                    Market = res.Market,
                    State = res.State,
                    IsManagement = true
                }).ToList();

                smartSearchResponse = managementResponse.Concat(propertyResponse).ToList();
            }
            else
            {
                var searchManagementResponse = await _client.SearchAsync<ManagementModel>(a => a.Size(25)
                    .From(0)
                    .Query(b => b
                        .Bool(c => c
                        .Must(o => o
                            .Match(f => f
                                .Field(fl => fl.Name)
                                .Query(searchPhrase))))));


                var searchPropertyResponse = await _client.SearchAsync<PropertiesModel>(s => s
                    .Size(25)
                    .From(0)
                    .Query(q => q.Bool(c => c
                        .Must(o => o
                        .MultiMatch(f => f
                            .Fields(f => f
                            .Field(f => f.FormerName)
                            .Field(f => f.State)
                            .Field(f => f.Market)
                            .Field(f => f.StreetAddress))
                                .Query(searchPhrase))))));
                propertyResponse = searchPropertyResponse?.Documents.Select(res => new SearchListContentVm()
                {
                    Name = res.Name,
                    Market = res.Market,
                    State = res.State,
                    IsManagement = false
                }).ToList();

                managementResponse = searchManagementResponse?.Documents.Select(res => new SearchListContentVm()
                {
                    Name = res.Name,
                    Market = res.Market,
                    State = res.State,
                    IsManagement = true
                }).ToList();

                smartSearchResponse = managementResponse.Concat(propertyResponse).ToList();

            }

            smartSearchResponse = smartSearchResponse.Count > 40  ? smartSearchResponse.Take(10).Concat(smartSearchResponse.TakeLast(15)).ToList() : smartSearchResponse;

            return smartSearchResponse;
        }

    }
}
