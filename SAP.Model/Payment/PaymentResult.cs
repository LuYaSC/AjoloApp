using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Payment
{
    public class PaymentResult
    {
        public string Parent { get; set; }

        public string Kid { get; set; }

        public string Collaborator { get; set; }

        public string Room { get; set; }

        public string Turn { get; set; }

        public string Modality { get; set; }

        public string BranchOffice { get; set; }

        public string PaymentType { get; set; }

        public string PaymentOperation { get; set; }

        public string AuditPayment { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string NumberBill { get; set; }

        public string Observations { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
