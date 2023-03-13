using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SAP.Model.Authentication;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAP.RuleEngine.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;
        SAPContext Context;

        public AuthenticationService(UserManager<User> userManager, SAPContext Context, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.Context = Context;
        }

        private LoginResult GetResponse(bool isValid, string message, string? token = null, DateTime? expirate = null)
        {
            return new LoginResult { IsValid = isValid, Message = message, Token = token, Expirate = expirate };
        }

        public async Task<LoginResult> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return await Task.FromResult(GetResponse(false, "La cuenta ingresada no existe, favor revisar su cuenta e intentar nuevamente"));
            }

            if (user.AccessFailedCount == 3 && user.State != "B")
            {
                user.State = "B";
                Context.SaveChanges();
                return await Task.FromResult(GetResponse(false, "Cuenta Bloqueada comuniquese con el area de sistemas"));
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
                await Context.SaveChangesAsync();
                return await Task.FromResult(GetResponse(false, "La contraseña ingresada no  es correcta, favor intentar nuevamente"));
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
                expires: DateTime.UtcNow.AddHours(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            user.AccessFailedCount = 0;
            Context.Sessions.Add(new Session
            {
                Action = "Ingreso correcto",
                Name = model.Username,
                Password = user.PasswordHash,
                DateCreation = DateTime.UtcNow
            });
            await Context.SaveChangesAsync();

            return await Task.FromResult(GetResponse(true, "success", new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo));
        }

        public async Task<RegisterResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return await Task.FromResult(new RegisterResult { IsValid = false, Message = "User already exists!" });
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            User user = new User()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                AvailableDays = 120,
                DateCreation = DateTime.UtcNow,
                IsActive = true,
                State = "C"
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return await Task.FromResult(new RegisterResult { IsValid = false, Message = result.Errors.First().Description });
          
            var newUser = await Context.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            if (model.HaveDetail)
            {
                var userDetail = new UserDetail()
                {
                    BranchOfficeId = model.BranchOfficeId,
                    CityId = model.CityId,
                    DateCreation = DateTime.UtcNow,
                    UserId = newUser.Id
                };
                await Context.AddAsync(userDetail);
                await Context.SaveChangesAsync();
            }

            foreach (var role in model.Roles)
            {
                UserRole userRole = new UserRole
                {
                    DateCreation = DateTime.UtcNow,
                    RoleId = role,
                    UserId = newUser.Id
                };
                await Context.UserRoles.AddAsync(userRole);
                await Context.SaveChangesAsync();
            }

            if (!result.Succeeded)
                return await Task.FromResult(new RegisterResult { IsValid = false, 
                                             Message = "User creation failed! Please check user details and try again." });

            return await Task.FromResult(new RegisterResult
            {
                IsValid = true,
                Message = "User created successfully!",
                UserId = newUser.Id
            });
        }
    }
}
