using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Roles;
using SAP.RuleEngine.TypeBusinessService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class RolesController : ControllerBase
    {
        IRolesBusinessService service;

        public RolesController(IRolesBusinessService service)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<RolesResult> GetById([FromBody] GetRolesDto dto) => service.GetById(dto);

        [HttpGet]
        public Result<List<RolesResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Update([FromBody] RolesDto dto) => service.Update(dto);

        [HttpPost]
        public Result<string> Create([FromBody] RolesDto dto) => service.Create(dto);
    }
}
