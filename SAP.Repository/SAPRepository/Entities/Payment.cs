using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Payment : BaseTrace
    {
        public int AssignedRoomId { get; set; }

        [ForeignKey("AssignedRoomId")]
        public AssignedRoom AssignedRoom { get; set; }

        public int PaymentTypeId { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentType { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public string Observations { get; set; }
    }
}
