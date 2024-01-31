using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidBirthBackground: KidBackgroundBase
    {
        public int ManyMonths { get; set; }

        public bool HaveBreakupOfTheBag { get; set; }

        public bool Premature { get; set; }

        public bool Postmature { get; set; }

        public bool NormalDelivery { get; set; }

        public bool Fast { get; set; }

        public bool Delayed { get; set; }

        public string HowManyTime { get; set; } = string.Empty;

        public bool Induced { get; set; }

        public bool MotherReceivedAnesthesia { get; set; }

        public bool BornInHospital { get; set; }

        public bool BornInHome { get; set; }

        public string WasAttendedBy { get; set; } = string.Empty;

        public string HeadPositionAtBirth { get; set; } = string.Empty;

        public string FeetPositionAtBirth { get; set; } = string.Empty;

        public string ButtocksPositionAtBirth { get; set; } = string.Empty;
    }
}
