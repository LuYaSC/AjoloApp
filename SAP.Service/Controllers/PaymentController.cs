using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAP.Core.Business;
using SAP.Model.Payment;
using SAP.RuleEngine.PaymentService;

namespace SAP.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        IPaymentService service;

        public PaymentController(IPaymentService service)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<List<PaymentResult>> GetAll() => service.GetAll();

        [HttpPost]
        public Result<string> Create([FromBody] List<CreatePaymentDto> dto) => service.Create(dto);

        [HttpPost]
        public Result<string> Update([FromBody] List<UpdatePaymentDto> dto) => service.Update(dto);

        [HttpPost]
        public Result<PaymentDetailResult> GetDetail([FromBody] PaymentDetailDto dto) => service.GetDetail(dto);

        [HttpPost]
        public Result<List<PaymentResult>> GetFilter([FromBody] PaymentFilterDto dto) => service.GetFilter(dto);

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] PaymentDetailDto dto) => service.ActivateOrDeactivate(dto);
    }
}
