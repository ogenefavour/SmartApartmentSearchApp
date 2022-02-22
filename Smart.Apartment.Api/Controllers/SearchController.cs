using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smart.Apartment.Application.Features.Markets.Query.GetMarketList;
using Smart.Apartment.Application.Features.Searches.Queries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace Smart.Apartment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Route("GetMarketList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MarketListVm>> GetMarketList()
        {
            var dtos = await _mediator.Send(new GetMarketListQuery());
            return Ok(dtos);
        }

        [Route("SearchSmartApartment")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<SearchListContentVm>>> SearchSmartApartment([Required]string searchPhrase, string market)
        {
            var dtos = await _mediator.Send(new GetSearchQuery() { searchPhrase  = searchPhrase, Market = market });
            return Ok(dtos);
        }
    }
}
