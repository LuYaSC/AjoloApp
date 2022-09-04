﻿using SAP.Repository.SAPRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class AssignedRoom : BaseTrace
    {
        public int CollaboratorId { get; set; }

        [ForeignKey("CollaboratorId")]
        public virtual Collaborator Collaborator { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public int TurnId { get; set; }

        [ForeignKey("TurnId")]
        public virtual Turn Turn { get; set; }

        public int ModalityId { get; set; }

        [ForeignKey("ModalityId")]
        public virtual Modality Modality { get; set; }

        public int BranchOfficeId { get; set; }

        [ForeignKey("BranchOfficeId")]
        public virtual BranchOffice BranchOffice { get; set; }

        public string Observations { get; set; }
    }
}
