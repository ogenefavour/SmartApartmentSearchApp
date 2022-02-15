using Microsoft.Extensions.Options;
using Nest;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.Application.Features.Searches.Queries;
using Smart.Apartment.Application.Models;
using Smart.Apartment.Application.Models.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Apartment.Infrastructure.Search
{
    public class SearchService : ISearchService
    {
        private readonly EndPoints _endPoints;
        private readonly IElasticClient _client;
        public SearchService(IOptions<EndPoints> endPoints, IElasticClient client)
        {
            _endPoints = endPoints.Value ?? throw new ArgumentNullException(nameof(endPoints));
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

        public async Task<ICollection<SearchListContentVm>> SearchSmartAparment(string searchPhrase, string market)
        {
            var smartSearchResponse = new List<SearchListContentVm>();
            if (!string.IsNullOrEmpty(market))
            {
                  var searchResponse = await _client.SearchAsync<object>(s => s
                        .From(0)
                        .Query(q => q.Bool(b => b
                            .Must(m => m
                            .MultiMatch(mu => mu
                                .Fields(ff => ff
                                    .Field(Infer.Field<ManagementModel>(f => f.Name))
                                    .Field(Infer.Field<ManagementModel>(f => f.Market))
                                    .Field(Infer.Field<ManagementModel>(f => f.State))
                                )
                             ) && +q
                            .MultiMatch(mu => mu
                                .Fields(ff => ff
                                    .Field(Infer.Field<PropertiesModel>(f => f.Name))
                                    .Field(Infer.Field<PropertiesModel>(f => f.FormerName))
                                    .Field(Infer.Field<PropertiesModel>(f => f.State))
                                    .Field(Infer.Field<PropertiesModel>(f => f.Market))
                                    .Field(Infer.Field<PropertiesModel>(f => f.StreetAddress))
                                )
                                .Query(searchPhrase)), mn => mn
                                .Match(m => m
                                    .Field(Infer.Field<PropertiesModel>(f => f.Market))
                                    .Field(Infer.Field<ManagementModel>(f => f.Market))
                                    .Query(market))
                                ))));
                smartSearchResponse = searchResponse.Documents.Select(res => new SearchListContentVm()
                {
                    Name = (string)res.GetType().GetProperty("").GetValue(res, null),
                    Market = (string)res.GetType().GetProperty("").GetValue(res, null),
                    State = (string)res.GetType().GetProperty("").GetValue(res, null),
                    IsManagement = res.GetType() == typeof(ManagementModel) ? true : false
                }).ToList();


            }
            else
            {
                var searchResponse = await _client.SearchAsync<object>(s => s
                        .From(0)
                        .Query(q => q.Bool(b => b
                            .Must(m => m
                            .MultiMatch(mu => mu
                                .Fields(ff => ff
                                    .Field(Infer.Field<ManagementModel>(f => f.Name))
                                    .Field(Infer.Field<ManagementModel>(f => f.Market))
                                    .Field(Infer.Field<ManagementModel>(f => f.State))
                                )
                             ) && +q
                            .MultiMatch(mu => mu
                                .Fields(ff => ff
                                    .Field(Infer.Field<PropertiesModel>(f => f.Name))
                                    .Field(Infer.Field<PropertiesModel>(f => f.FormerName))
                                    .Field(Infer.Field<PropertiesModel>(f => f.State))
                                    .Field(Infer.Field<PropertiesModel>(f => f.Market))
                                    .Field(Infer.Field<PropertiesModel>(f => f.StreetAddress))
                                )
                                .Query(searchPhrase))
                                ))));
                smartSearchResponse = searchResponse.Documents.Select(res => new SearchListContentVm()
                {
                    Name = (string)res.GetType().GetProperty("").GetValue(res, null),
                    Market = (string)res.GetType().GetProperty("").GetValue(res, null),
                    State = (string)res.GetType().GetProperty("").GetValue(res, null),
                    IsManagement = res.GetType() == typeof(ManagementModel) ? true : false
                }).ToList();
            }
             
            return smartSearchResponse;
        }

    }
}
