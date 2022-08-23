using Microsoft.AspNetCore.Identity;
using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Role : IdentityRole<int>, ILogicalDelete, IAuditable<int, int>
    {
        public int UserCreation { get; set; }
        public int UserModification { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DateModification { get; set; }
        public bool IsDeleted { get; set; }
    }
}
