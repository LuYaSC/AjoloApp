﻿using Microsoft.AspNetCore.Identity;
using SAP.Repository.SAPRepository.Base;

namespace SAP.Repository.SAPRepository.Entities
{
    public class UserRole : IdentityUserRole<int>, ILogicalDelete, IDateCreation, IDateModification
    {
        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
