using SAP.Repository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class BranchOffice : BaseType
    {
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
