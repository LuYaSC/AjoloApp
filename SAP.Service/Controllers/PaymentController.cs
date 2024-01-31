using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.Payment;
using AjoloApp.RuleEngine.PaymentService;
using AjoloApp.Service.Models;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class PaymentController : BaseController
    {
        IPaymentService service;

        public PaymentController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IPaymentService service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpGet]
        public Result<List<PaymentResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] CreatePaymentDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] UpdatePaymentDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<PaymentDetailResult> GetDetail([FromBody] PaymentDetailDto dto)
        {
            var result = service.GetDetail(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<List<PaymentResult>> GetFilter([FromBody] PaymentFilterDto dto)
        {
            var result = service.GetFilter(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> ActivateOrDeactivate([FromBody] PaymentDetailDto dto)
        {
            var result = service.ActivateOrDeactivate(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<ReportResult> GenerateReport([FromBody] PaymentFilterDto dto)
        {
            var result = service.GeneratePdf(dto, $"Lista de Pagos ({DateTime.UtcNow.Year})");
            SaveRequest(dto, result);
            return result;
        }
    }
}
