﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Parent
{
    public class CreateParentDto
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string? PlaceBorn { get; set; }

        public string BloodType { get; set; }

        public string DocumentNumber { get; set; }

        public int DocumentTypeId { get; set; }

        public string Sex { get; set; }
    }
}