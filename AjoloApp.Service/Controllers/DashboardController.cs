using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.Dashboard;
using AjoloApp.RuleEngine.DashboardService;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class DashboardController : ControllerBase
    {
        IDashboardService service;

        public DashboardController(IDashboardService service)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<DashboardResult> GetData() => service.GetData();
    }
}
