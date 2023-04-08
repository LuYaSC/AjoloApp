using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.RuleEngine.AssignationRoomService;
using SAP.Service.Models;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AssignationRoomController : BaseController
    {
        IAssignationRoomService service;

        public AssignationRoomController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IAssignationRoomService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<List<AssignationRoomResult>> GetFilter([FromBody] CreateAssignedRoomDto dto)
        {
            var result = service.GetFilter(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<AssignationRoomResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] CreateAssignedRoomDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] UpdateAssignedRoomDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> DisableOrEnable([FromBody] UpdateAssignedRoomDto dto)
        {
            var result = service.DisableOrEnable(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<AssignationRoomDetailResult> GetDetail([FromBody] AssignationRoomDetailDto dto)
        {
            var result = service.GetDetail(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf("Lista de Salas / Colaboradores Asignados");
            SaveRequest(response: result);
            return result;
        }
    }
}
