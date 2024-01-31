using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationTutor;
using AjoloApp.RuleEngine.AssignationTutorService;
using AjoloApp.Service.Models;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AssignationTutorController : BaseController
    {
        IAssignationTutorService service;

        public AssignationTutorController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IAssignationTutorService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<string> DisableOrEnable([FromBody] UpdateAssignedTutorDto dto)
        {
            var result = service.DisableOrEnable(dto);
            SaveRequest(dto, result);
            return result;
        }


        [HttpPost]
        public Result<List<AssignationTutorResult>> GetFilter([FromBody] CreateAssignedTutorDto dto)
        {
            var result = service.GetFilter(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<AssignationTutorResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] CreateAssignedTutorDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] UpdateAssignedTutorDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf("Lista de Padres / Niños");
            SaveRequest(response: result);
            return result;
        }
    }
}
