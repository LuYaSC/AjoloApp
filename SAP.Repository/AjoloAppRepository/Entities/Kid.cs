using AjoloApp.Repository.AjoloAppRepository.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class Kid : BaseTrace, IName, IFirstLastName, ISecondLastName, ISexType
    {
        public string Name { get; set; } = string.Empty;

        public string FirstLastName { get; set; } = string.Empty;

        public string? SecondLastName { get; set; }

        public int SexTypeId { get; set; }

        [ForeignKey("SexTypeId")]

        public SexType SexType { get; set; }

        public DateTime? BornDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PlaceBorn { get; set; } = string.Empty;

        public int BloodTypeId { get; set; }

        [ForeignKey("BloodTypeId")]

        public BloodType BloodType { get; set; }

        public string DocumentNumber { get; set; } = string.Empty;

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        
        public DocumentType DocumentType { get; set; }
    }
}
