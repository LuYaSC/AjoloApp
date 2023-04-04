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

        [HttpPost]
        public Result<string> DisableOrEnable([FromBody] UpdateAssignedTutorDto dto) => service.DisableOrEnable(dto);

        [HttpPost]
        public Result<List<AssignationTutorResult>> GetFilter([FromBody] CreateAssignedTutorDto dto) => service.GetFilter(dto);

        [HttpGet]
        public Result<List<AssignationTutorResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] CreateAssignedTutorDto dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] UpdateAssignedTutorDto dto) => service.Update(dto);

        [HttpGet]
        public Result<ReportResult> GenerateReport() => service.GeneratePdf("Lista de Padres / Niños");
    }
}
