using Moq;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.UnitTests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Smart.Apartment.UnitTests.FeatureTests.Market
{
    public class GetMarketListQueryHandlerTest
    {
        private readonly Mock<ISearchService> _marketListService;
        public GetMarketListQueryHandlerTest()
        {
            _marketListService = ConfigureMock.GetSearchMarketListServiceMock();
        }

        [Fact]
        public async Task GetMarketListQueryHandlerTestShouldReturnValidType()
        {
            var handler = new GetMarketListQueryHandler(_marketListService.Object);

            var result = await handler.Handle(new GetMarketListQuery(), CancellationToken.None);

            Assert.IsType<List<MarketListVm>>(result);

            Assert.Equal(4, result.Count);
        }
    }
    
}
