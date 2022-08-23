using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class AssignedTutor : BaseBackOfficeTrace
    {
        public int KidId { get; set; }

        public int ParentId { get; set; }

        public int AssignedRoom { get; set; }
    }
}
