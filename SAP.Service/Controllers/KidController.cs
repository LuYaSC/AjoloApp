using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SAP.Service.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]/[action]")]
    public class KidController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return string.Empty;
        }
    }
}
