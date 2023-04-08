using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.Model.EnrolledChildren;
using SAP.RuleEngine.EnrolledChildrenService;
using SAP.Service.Models;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class EnrolledChildrenController : BaseController
    {
        IEnrolledChildrenService service;

        public EnrolledChildrenController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IEnrolledChildrenService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<List<EnrolledChildrenResult>> GetFilter([FromBody] EnrollFilterDto dto)
        {
            var result = service.GetFilter(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<EnrollChildrenDetailResult> GetDetail([FromBody] UpdateAssignedRoomDto dto)
        {
            var result = service.GetDetail(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<EnrolledChildrenResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] CreateEnrolledChildrenDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] UpdateEnrolledChildrenDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] UpdateEnrolledChildrenDto dto)
        {
            var result = service.ActivateOrDeactivate(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf($"Lista de Inscritos ({DateTime.UtcNow.Year})");
            SaveRequest(response: result);
            return result;
        }
    }
}
