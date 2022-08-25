using Microsoft.AspNetCore.Identity;
using SAP.Repository.SAPRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class UserRole : IdentityUserRole<int>, ILogicalDelete, IDateCreation, IDateModification
    {

        public int BranchOfficeId { get; set; }

        [ForeignKey("BranchOfficeId")]
        public BranchOffice BranchOffice { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
