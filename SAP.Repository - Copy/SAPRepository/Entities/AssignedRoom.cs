using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class AssignedRoom : BaseBackOfficeTrace
    {
        public int KidId { get; set; }

        public int RoomId { get; set; }

        public int TurnId { get; set; }
    }
}
