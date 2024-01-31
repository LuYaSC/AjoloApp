using Microsoft.AspNetCore.Mvc;
using AjoloApp.Model.Authentication;

namespace AjoloApp.RuleEngine.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(LoginModel model);

        Task<RegisterResult> Register([FromBody] RegisterModel model);
    }
}
