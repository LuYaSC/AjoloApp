using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Payment : BaseBackOfficeTrace
    {
        public int AssignedTutorId { get; set; }

        public decimal Amount { get; set; }
    }
}
