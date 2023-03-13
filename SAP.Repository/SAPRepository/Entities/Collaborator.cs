using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Collaborator : BaseTrace, IName, IFirstLastName, ISecondLastName, ISexType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        [Required]
        public DateTime BornDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DocumentNumber { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentType { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]

        public User User { get; set; }

        public int SexTypeId { get; set; }

        [ForeignKey("SexTypeId")]

        public SexType SexType { get; set; }

        public int BloodTypeId { get; set; }

        [ForeignKey("BloodTypeId")]

        public BloodType BloodType { get; set; }
    }
}
