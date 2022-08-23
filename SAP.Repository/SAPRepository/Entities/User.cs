using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SAP.Repository.SAPRepository.Entities
{
    public class User : IdentityUser<int>
    {
        public int AvailableDays { get; set; } = 0;

        [Required]
        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public string State { get; set; }

        public bool IsActive { get; set; }
    }
}
