using SAP.Repository.SAPRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class EnrolledChildren : BaseTrace
    {
        public int AssignedTutorId { get; set; }

        [ForeignKey("AssignedTutorId")]
        public virtual AssignedTutor AssignedTutor { get; set; }

        public int AssignedRoomId { get; set; }

        [ForeignKey("AssignedRoomId")]
        public virtual AssignedRoom AssignedRoom { get; set; }

        public string Observations { get; set; }
    }
}
