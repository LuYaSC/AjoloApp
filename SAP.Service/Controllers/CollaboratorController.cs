using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Collaborator;
using SAP.Repository.SAPRepository.Entities;
using SAP.RuleEngine.CollaboratorService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CollaboratorController : ControllerBase
    {
        ICollaboratorService service;

        public CollaboratorController(ICollaboratorService service)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<CollaboratorsResult> GetCollaboratorById([FromBody] CollaboratorByIdDto dto) => service.GetCollaboratorById(dto);

        //[HttpPost]
        //public Result<CollaboratorsResult> GetCollaborator([FromBody] GetCollaboratorDto dto) => service.GetCollaborators(dto);

        [HttpGet]
        public Result<List<CollaboratorsResult>> GetAllCollaborators() => service.GetCollaborators();

        [HttpPost]
        public Result<string> CreateCollaborator([FromBody] CreateCollaboratorDto dto) => service.CreateCollaborator(dto);

        [HttpPost]
        public Result<string> UpdateCollaborator([FromBody] UpdateCollaboratorDto dto) => service.UpdateCollaborator(dto);

        [HttpPost]
        public Result<string> ActivateOrDeactivate(DeleteDto dto) => service.ActivateOrDeactivate(dto);

        [HttpGet]
        public Result<ReportResult> GenerateReport() => service.GeneratePdf("LISTA DE COLABORADORES");
    }
}
