﻿using AjoloApp.Repository.AjoloAppRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class AssignedTutor : BaseTrace
    {
        public int KidId { get; set; }

        [ForeignKey("KidId")]
        public Kid Kid { get; set; }

        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Parent Parent { get; set; }

        public int RelationshipId { get; set; }

        [ForeignKey("RelationshipId")]
        public Relationship Relationship { get; set; }

        public string Observations { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
