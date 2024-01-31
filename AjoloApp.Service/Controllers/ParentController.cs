using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.Authentication;
using AjoloApp.Model.Parent;
using AjoloApp.RuleEngine.ParentService;
using AjoloApp.Service.Models;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ParentController : BaseController
    {
        IParentService service;

        public ParentController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IParentService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<List<ParentsResult>> GetAllParents()
        {
            var result = service.GetAllParents();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<ParentsResult> GetParentById([FromBody] ParentByIdDto dto)
        {
            var result = service.GetParentById(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<ParentsResult> GetParent([FromBody] GetParentDto dto)
        {
            var result = service.GetParent(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> CreateParent([FromBody] CreateParentDto dto)
        {
            var result = service.CreateParent(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> UpdateParent([FromBody] UpdateParentDto dto)
        {
            var result = service.UpdateParent(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> ActivateOrDeactivate(DeleteDto dto)
        {
            var result = service.ActivateOrDeactivate(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf("LISTA DE PADRES");
            SaveRequest(response: result);
            return result;
        }
    }
}
