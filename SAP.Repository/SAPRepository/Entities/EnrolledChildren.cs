using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class EnrolledChildren : BaseTrace
    {
        public int AssignedTutorId { get; set; }

        [ForeignKey("AssignedTutorId")]
        public AssignedTutor AssignedTutor { get; set; }

        public int AssignedRoomId { get; set; }

        [ForeignKey("AssignedRoomId")]
        public AssignedRoom AssignedRoom { get; set; }
    }
}
