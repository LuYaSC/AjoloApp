namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidBackground : KidBackgroundBase
    {
        public int AgeMotherDuringGestation { get; set; }

        public bool IsPlanified { get; set; }

        public bool ThreatOfAbortion { get; set; }

        public bool PrenatalCheckUp { get; set; }

        public bool UrineTestDone { get; set; }

        public bool BloodTestDone { get; set; }

        public string OtherTests { get; set; } = string.Empty;

        public bool XRays3rdMonth { get; set; }

        public bool DrinkDuringGestation { get; set; }

        public string MedicinesConsumed { get; set; } = string.Empty;

        public string PhysicalConditionsDuringGestation { get; set; } = string.Empty;

        public string PsychologicalConditionsDuringGestation { get; set; } = string.Empty;
    }
}
