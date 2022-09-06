using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationTutor;
using SAP.RuleEngine.AssignationTutorService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AssignationTutorController : ControllerBase
    {
        IAssignationTutorService service;

        public AssignationTutorController(IAssignationTutorService service)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<List<AssignationTutorResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] List<CreateAssignedTutorDto> dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] List<UpdateAssignedTutorDto> dto) => service.Update(dto);
    }
}
