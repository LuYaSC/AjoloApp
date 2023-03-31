using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
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

        [HttpPost]
        public Result<List<EnrolledChildrenResult>> GetFilter([FromBody] EnrollFilterDto dto) => service.GetFilter(dto);

        [HttpPost]
        public Result<EnrollChildrenDetailResult> GetDetail([FromBody] UpdateAssignedRoomDto dto) => service.GetDetail(dto);

        [HttpGet]
        public Result<List<EnrolledChildrenResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] CreateEnrolledChildrenDto dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] UpdateEnrolledChildrenDto dto) => service.Update(dto);

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] UpdateEnrolledChildrenDto dto) => service.ActivateOrDeactivate(dto);
    }
}
