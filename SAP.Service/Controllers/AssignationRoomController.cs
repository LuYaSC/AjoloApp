using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.Model.AssignationTutor;
using SAP.RuleEngine.AssignationRoomService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AssignationRoomController : ControllerBase
    {
        IAssignationRoomService service;

        public AssignationRoomController(IAssignationRoomService service)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<List<AssignationRoomResult>> GetFilter([FromBody] CreateAssignedRoomDto dto) => service.GetFilter(dto);

        [HttpGet]
        public Result<List<AssignationRoomResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] CreateAssignedRoomDto dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] UpdateAssignedRoomDto dto) => service.Update(dto);

        [HttpPost]
        public Result<string> DisableOrEnable([FromBody] UpdateAssignedRoomDto dto) => service.DisableOrEnable(dto);

        [HttpPost]
        public Result<AssignationRoomDetailResult> GetDetail([FromBody] AssignationRoomDetailDto dto) => service.GetDetail(dto);
    }
}
