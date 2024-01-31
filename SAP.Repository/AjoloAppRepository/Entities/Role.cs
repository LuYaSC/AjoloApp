using Microsoft.AspNetCore.Identity;
using AjoloApp.Repository.AjoloAppRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class Role : IdentityRole<int>, ILogicalDelete
    {
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public bool IsDeleted { get; set; }
    }
}
