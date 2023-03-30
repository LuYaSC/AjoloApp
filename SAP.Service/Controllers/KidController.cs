using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Kid;
using SAP.RuleEngine.KidService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class KidController : ControllerBase
    {
        IKidService service;

        public KidController(IKidService service)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<KidsResult> GetKidById([FromBody] KidByIdDto dto) => service.GetKidById(dto);

        [HttpPost]
        public Result<KidsResult> GetKid([FromBody] GetKidDto dto) => service.GetKid(dto);

        [HttpGet]
        public Result<List<KidsResult>> GetAllKids() => service.GetAllKids();

        [HttpPost]
        public Result<string> CreateKid([FromBody] CreateKidDto dto) => service.CreateKid(dto);

        [HttpPost]
        public Result<string> UpdateKid([FromBody] UpdateKidDto dto) => service.UpdateKid(dto);

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] KidByIdDto dto) => service.ActivateOrDeactivate(dto);

        [HttpPost]
        public Result<List<GetDetailKidResult>> GetDetailKid([FromBody] GetDetailKidDto dto) => service.GetDetailKid(dto);
    }
}
