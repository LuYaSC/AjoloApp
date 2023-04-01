using SAP.Core.Business;
using SAP.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.PaymentService
{
    public interface IPaymentService
    {
        Result<List<PaymentResult>> GetFilter(PaymentFilterDto dto);

        Result<List<PaymentResult>> GetAll();

        Result<PaymentDetailResult> GetDetail(PaymentDetailDto dto);

        Result<string> Create(List<CreatePaymentDto> dto);

        Result<string> Update(List<UpdatePaymentDto> dto);

        Result<string> ActivateOrDeactivate(PaymentDetailDto dto);
    }
}
