using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Parent
{
    public class ParentsResult
    {
        public int Id { get; set; }
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

        public string DocumentType { get; set; }

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

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
