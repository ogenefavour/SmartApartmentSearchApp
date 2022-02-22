using Moq;
using Smart.Apartment.Application.Contracts.Infrastructure;
using Smart.Apartment.Application.Features.Searches.Queries;
using Smart.Apartment.UnitTests.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Smart.Apartment.UnitTests.FeatureTests.Search
{
    public class GetSearchQueryHandlerTests
    {
        private readonly Mock<ISearchService> _searchServiceMock;
        public GetSearchQueryHandlerTests()
        {
            _searchServiceMock = ConfigureMock.GetSearchSearchServiceMock();
        }
        [Fact]
        public async Task GetSearchQueryHandlerTest()
        {
            var handler = new GetSearchQueryHandler(_searchServiceMock.Object);

            var result = await handler.Handle(new GetSearchQuery(), CancellationToken.None);

            Assert.IsType<List<SearchListContentVm>>(result);
        }
    }
}
