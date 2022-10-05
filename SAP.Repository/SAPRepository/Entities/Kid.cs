using SAP.Repository.SAPRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Kid : BaseTrace, IName, IFirstLastName, ISecondLastName, ISex
    {
        public string Name { get; set; } = string.Empty;

        public string FirstLastName { get; set; } = string.Empty;

        public string? SecondLastName { get; set; }

        public string Sex { get; set; } = string.Empty;

        public DateTime? BornDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PlaceBorn { get; set; } = string.Empty;

        public string BloodType { get; set; } = string.Empty;

        public string DocumentNumber { get; set; } = string.Empty;

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        
        public DocumentType DocumentType { get; set; }
    }
}
