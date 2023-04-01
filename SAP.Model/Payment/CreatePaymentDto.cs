using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Payment
{
    public class CreatePaymentDto
    {
        public int EnrolledChildrenId { get; set; }

        public int PaymentTypeId { get; set; }

        public int PaymentOperationId { get; set; }

        public int AuditPaymentId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string NumberBill { get; set; }

        public string Observations { get; set; }

        public bool IsVerified { get; set; }
    }
}
