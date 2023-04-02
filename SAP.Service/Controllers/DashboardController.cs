using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Dashboard;
using SAP.RuleEngine.DashboardService;

namespace SAP.Service.Controllers
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
