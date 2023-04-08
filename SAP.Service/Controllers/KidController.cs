using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Kid;
using SAP.RuleEngine.KidService;
using SAP.Service.Models;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class KidController : BaseController
    {
        IKidService service;

        public KidController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IKidService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<KidsResult> GetKidById([FromBody] KidByIdDto dto)
        {
            var result = service.GetKidById(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<KidsResult> GetKid([FromBody] GetKidDto dto)
        {
            var result = service.GetKid(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<KidsResult>> GetAllKids()
        {
            var result = service.GetAllKids();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> CreateKid([FromBody] CreateKidDto dto)
        {
            var result = service.CreateKid(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> UpdateKid([FromBody] UpdateKidDto dto)
        {
            var result = service.UpdateKid(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] KidByIdDto dto)
        {
            var result = service.ActivateOrDeactivate(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<List<GetDetailKidResult>> GetDetailKid([FromBody] GetDetailKidDto dto)
        {
            var result = service.GetDetailKid(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf("LISTA DE NIÑOS");
            SaveRequest(response: result);
            return result;
        }
    }
}
