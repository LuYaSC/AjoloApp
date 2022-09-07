using Microsoft.AspNetCore.Mvc;
using SAP.Model.Authentication;
using SAP.RuleEngine.AuthenticationService;


namespace SAM.Functions.Authorization.MicroService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AuthenticateController : ControllerBase
    {
        IAuthenticationService service;

        public AuthenticateController(IAuthenticationService service) => this.service = service;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await service.Login(model);
            return result.IsValid ? Ok(result) : Unauthorized(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await service.Register(model);
            return result.IsValid ? Ok(result) : StatusCode(StatusCodes.Status500InternalServerError, 
                                                            new Response { Status = "Error", Message = result.Message });
        }

        //[HttpPost]
        //[Route("UpdateUser")]
        //public async Task<IActionResult> UpdateUser([FromBody] RegisterModel model)
        //{
        //    var userExists = await userManager.FindByNameAsync(model.Username);
        //    if (userExists == null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User doesn't exists!" });

        //    return null;
        //}
    }
}
