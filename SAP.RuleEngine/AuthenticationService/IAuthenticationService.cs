using Microsoft.AspNetCore.Mvc;
using SAP.Model.Authentication;

namespace SAP.RuleEngine.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(LoginModel model);

        Task<RegisterResult> Register([FromBody] RegisterModel model);
    }
}
