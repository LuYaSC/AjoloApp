using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.Authentication;
using AjoloApp.RuleEngine.AuthenticationService;


namespace SAM.Functions.Authorization.MicroService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AuthenticateController : ControllerBase
    {
        IAuthenticationService service;

        public AuthenticateController(IAuthenticationService service) => this.service = service;

        [HttpPost]
        public async Task<Result<LoginResult>> Login([FromBody] LoginModel model)
        {
            var result = await service.Login(model);
            return result.IsValid ? Result<LoginResult>.SetOk(result) : Result<LoginResult>.SetError(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await service.Register(model);
            return result.IsValid ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError,
                                                            new Response { Status = "Error", Message = result.Message });
        }
    }
}
