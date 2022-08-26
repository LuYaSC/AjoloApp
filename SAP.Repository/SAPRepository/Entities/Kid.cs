using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Kid : BaseTrace, IName, IFirstLastName, ISecondLastName, ISex
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

        public int AgePregnancyMother { get; set; }

        public bool IsPlanified { get; set; }

        public bool ThreatenedAbortion { get; set; }

        public bool PrenatalCheckup { get; set; }

        public bool UrineTest { get; set; }

        public string XRaysThirdMonth { get; set; }

        public string MotherDrink { get; set; }

        public string Medicaments { get; set; }

        public string PhysicalsConditions { get; set; }

        public string PsychologicalConditions { get; set; }
    }
}
