using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Kid : BaseTrace
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public string Sex { get; set; }

        public DateTime? BornDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PlaceBorn { get; set; }

        public string BloodType { get; set; }

        public string DocumentNumber { get; set; }

        public int DocumentTypeId { get; set; }

        [ForeignKey("DocumentTypeId")]

        public DocumentType DocumentType { get; set; }

        public int KidBackgroudId { get; set; }

        [ForeignKey("KidBackgroudId")]

        public KidBackground KidBackground { get; set; }

        public bool IsActive { get; set; }
    }
}
