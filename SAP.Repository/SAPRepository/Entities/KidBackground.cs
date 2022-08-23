using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class KidBackground : BaseTrace
    {
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
