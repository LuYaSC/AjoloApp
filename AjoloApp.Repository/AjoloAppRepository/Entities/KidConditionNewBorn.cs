namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidConditionNewBorn : KidBackgroundBase
    {
        public decimal Weight { get; set; }

        public decimal Size { get; set; }

        public bool HeadLarge { get; set; }

        public bool HeadSmall { get; set; }

        public bool CryImmediately { get; set; }

        public bool NeedOxigen { get; set; }

        public bool UseIncubator { get; set; }

        public bool HadMalformations { get; set; }

        public string DescribeMalformations { get; set; } = string.Empty;

        public bool SufferedAnyDisease { get; set; }

        public string DescribeDisease { get; set; } = string.Empty;

        public int AgeDisease { get; set; }

        public string MedicamentsTreatDisease { get; set; } = string.Empty;

    }
}
