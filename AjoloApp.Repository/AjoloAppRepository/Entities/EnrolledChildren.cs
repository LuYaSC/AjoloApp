﻿using AjoloApp.Repository.AjoloAppRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
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
