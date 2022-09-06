using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.EnrolledChildren;
using SAP.RuleEngine.EnrolledChildrenService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class EnrolledChildrenController : ControllerBase
    {
        IEnrolledChildrenService service;

        public EnrolledChildrenController(IEnrolledChildrenService service)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<List<EnrolledChildrenResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] List<CreateEnrolledChildrenDto> dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] List<UpdateEnrolledChildrenDto> dto) => service.Update(dto);
    }
}
