using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class UserDetail: BaseData, IDateCreation, IDateModification
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int BranchOfficeId { get; set; }

        [ForeignKey("BranchOfficeId")]
        public BranchOffice BranchOffice { get; set; }

        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}
