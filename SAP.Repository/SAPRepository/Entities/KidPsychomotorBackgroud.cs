namespace SAP.Repository.SAPRepository.Entities
{
    public class KidPsychomotorBackgroud : KidBackgroundBase
    {
        public string AffirmTheHead { get; set; } = string.Empty;

        public string SatUp { get; set; } = string.Empty;

        public string CrawlAt { get; set; } = string.Empty;

        public string StandAt { get; set; } = string.Empty;

        public string WalkedTo { get; set; } = string.Empty;

        public bool HaveAnyDifficultiesInMovements { get; set; }

        public string DescribeDifficultiesMovements { get; set; } = string.Empty;
    }
}
