using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Collaborator;
using SAP.RuleEngine.CollaboratorService;
using SAP.Service.Models;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CollaboratorController : BaseController
    {
        ICollaboratorService service;

        public CollaboratorController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            ICollaboratorService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<CollaboratorsResult> GetCollaboratorById([FromBody] CollaboratorByIdDto dto)
        {
            var result = service.GetCollaboratorById(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<CollaboratorsResult>> GetAllCollaborators()
        {
            var result = service.GetCollaborators();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> CreateCollaborator([FromBody] CreateCollaboratorDto dto)
        {
            var result = service.CreateCollaborator(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> UpdateCollaborator([FromBody] UpdateCollaboratorDto dto)
        {
            var result = service.UpdateCollaborator(dto);
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
            var result = service.GeneratePdf("LISTA DE COLABORADORES");
            SaveRequest(response: result);
            return result;
        }
    }
}
