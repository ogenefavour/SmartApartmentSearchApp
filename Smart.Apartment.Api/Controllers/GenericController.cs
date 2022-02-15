using Microsoft.AspNetCore.Mvc;

namespace Smart.Apartment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }
    }
}
