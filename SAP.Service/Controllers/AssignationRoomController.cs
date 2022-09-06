using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
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

        [HttpGet]
        public Result<List<AssignationRoomResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] List<CreateAssignedRoomDto> dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] List<UpdateAssignedRoomDto> dto) => service.Update(dto);
    }
}
