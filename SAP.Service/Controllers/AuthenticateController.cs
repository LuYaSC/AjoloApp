using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SAP.Model.Authentication;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
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
        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;
        SAPContext Context;

        public AuthenticateController(UserManager<User> userManager, SAPContext Context, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.Context = Context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized("La cuenta ingresada no existe, favor revisar su cuenta e intentar nuevamente");
            }

            if (user.AccessFailedCount == 3 && user.State != "B")
            {
                user.State = "B";
                Context.SaveChanges();
                return Unauthorized("Cuenta Bloqueada comuniquese con el area de sistemas");
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                Context.Sessions.Add(new Session
                {
                    Action = "Ingreso invalido password no coincide",
                    Name = model.Username,
                    Password = model.Password,
                    DateCreation = DateTime.Now
                });
                user.AccessFailedCount++;
                Context.SaveChanges();
                return Unauthorized("La contraseña ingresada no  es correcta, favor intentar nuevamente");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim("userName", user.UserName),
                    new Claim("identifier", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("roles", userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            user.AccessFailedCount = 0;
            Context.Sessions.Add(new Session
            {
                Action = "Ingreso correcto",
                Name = model.Username,
                Password = user.PasswordHash,
                DateCreation = DateTime.Now
            });
            Context.SaveChanges();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AvailableDays = 120,
                DateCreation = DateTime.Now,
                IsActive = true,
                State = "C"
            };
            var result = await userManager.CreateAsync(user, model.Password);
            var newUser = Context.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();

            foreach (var role in model.Roles)
            {
                UserRole userRole = new UserRole
                {
                    DateCreation = DateTime.Now,
                    RoleId = role,
                    UserId = newUser.Id
                };
                Context.UserRoles.Add(userRole);
                Context.SaveChanges();
            }

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

    }
}
