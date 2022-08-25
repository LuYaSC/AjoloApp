using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class AssignedRoom : BaseTrace
    {
        public int CollaboratorId { get; set; }

        [ForeignKey("CollaboratorId")]
        public Collaborator Collaborator { get; set; }

        public int AssignedTutorId { get; set; }

        [ForeignKey("AssignedTutorId")]
        public AssignedTutor AssignedTutor { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public int TurnId { get; set; }

        [ForeignKey("TurnId")]
        public Turn Turn { get; set; }

        public int ModalityId { get; set; }

        [ForeignKey("ModalityId")]
        public Modality Modality { get; set; }

        public int BranchOfficeId { get; set; }

        [ForeignKey("BranchOfficeId")]
        public BranchOffice BranchOffice { get; set; }
    }
}
