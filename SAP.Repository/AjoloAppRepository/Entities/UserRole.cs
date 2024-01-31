using Microsoft.AspNetCore.Identity;
using AjoloApp.Repository.AjoloAppRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class UserRole : IdentityUserRole<int>, ILogicalDelete, IDateCreation, IDateModification
    {
        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
