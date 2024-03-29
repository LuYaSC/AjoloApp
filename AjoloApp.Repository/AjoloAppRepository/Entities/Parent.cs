﻿using AjoloApp.Repository.AjoloAppRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class Parent : BaseTrace, IName, IFirstLastName, ISecondLastName, ISexType
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string? PlaceBorn { get; set; }

        public int BloodTypeId { get; set; }

        [ForeignKey("BloodTypeId")]

        public BloodType BloodType { get; set; }


        public string DocumentNumber { get; set; }

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]

        public DocumentType DocumentType { get; set; }

        public int SexTypeId { get; set; }

        [ForeignKey("SexTypeId")]

        public SexType SexType { get; set; }

        public int MaritalStatusId { get; set; }

        [ForeignKey("MaritalStatusId")]

        public MaritalStatus MaritalStatus { get; set; }
    }
}
