using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SAP.Model.Authentication;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using SAP.RuleEngine.AuthenticationService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Functions.Authorization.MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticateController : ControllerBase
    {
        IAuthenticationService service;

        public AuthenticateController(IAuthenticationService service) => this.service = service;

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await service.Login(model);
            return result.IsValid ? Ok(result) : Unauthorized(result.Message);
        }

        [HttpPost]
        [Route("register")]
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
