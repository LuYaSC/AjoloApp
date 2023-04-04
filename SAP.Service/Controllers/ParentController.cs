using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Parent;
using SAP.Repository.SAPRepository.Entities;
using SAP.RuleEngine.ParentService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ParentController : ControllerBase
    {
        IParentService service;

        public ParentController(IParentService service)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<ParentsResult> GetParentById([FromBody] ParentByIdDto dto) => service.GetParentById(dto);

        [HttpPost]
        public Result<ParentsResult> GetParent([FromBody] GetParentDto dto) => service.GetParent(dto);

        [HttpGet]
        public Result<List<ParentsResult>> GetAllParents() => service.GetAllParents();

        [HttpPost]
        public Result<string> CreateParent([FromBody] CreateParentDto dto) => service.CreateParent(dto);

        [HttpPost]
        public Result<string> UpdateParent([FromBody] UpdateParentDto dto) => service.UpdateParent(dto);

        [HttpPost]
        public Result<string> ActivateOrDeactivate(DeleteDto dto) => service.ActivateOrDeactivate(dto);

        [HttpGet]
        public Result<ReportResult> GenerateReport() => service.GeneratePdf("LISTA DE PADRES");
    }
}
