using SAP.Repository.SAPRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Payment : BaseTrace
    {
        public int EnrolledChildrenId { get; set; }

        [ForeignKey("EnrolledChildrenId")]
        public virtual EnrolledChildren EnrolledChildren { get; set; }

        public int PaymentTypeId { get; set; }

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }

        public int PaymentOperationId { get; set; }

        [ForeignKey("PaymentOperationId")]
        public virtual PaymentOperation PaymentOperation { get; set; }

        public int AuditPaymentId { get; set; }

        [ForeignKey("AuditPaymentId")]
        public virtual AuditPaymentType AuditPaymentType { get; set; }

        public DateTime DateToPay { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string NumberBill { get; set; }

        public string Observations { get; set; }

        public bool IsVerified { get; set; }
    }
}
