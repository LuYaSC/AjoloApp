using AjoloApp.Core.Business;
using AjoloApp.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.RuleEngine.PaymentService
{
    public interface IPaymentService
    {
        Result<List<PaymentResult>> GetFilter(PaymentFilterDto dto);

        Result<List<PaymentResult>> GetAll();

        Result<PaymentDetailResult> GetDetail(PaymentDetailDto dto);

        Result<string> Create(CreatePaymentDto dto);

        Result<string> Update(UpdatePaymentDto dto);

        Result<string> ActivateOrDeactivate(PaymentDetailDto dto);

        Result<ReportResult> GeneratePdf(PaymentFilterDto dto, string title);
    }
}
