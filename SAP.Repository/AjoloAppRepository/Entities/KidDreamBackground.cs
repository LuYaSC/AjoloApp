using System;
using System.Runtime.Intrinsics.X86;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidDreamBackground : KidBackgroundBase
    {
        public bool BabyDreamCalm { get; set; }

        public bool BabyDreamShaken { get; set; }

        public bool CurrentlyDreamCalm { get; set; }

        public bool Shifts { get; set; }

        public bool Speaks { get; set; }

        public bool NocturnalFear { get; set; }

        public bool Breathes { get; set; }

        public bool ScratchesTeeth { get; set; }

        public bool WakesUpALot { get; set; }

        public bool Enuresis { get; set; }

        public bool HasSingleRoom { get; set; }

        public bool HasSingleBed { get; set; }

        public string WhoSleep { get; set; } = string.Empty;

        public string Bedtime { get; set; } = string.Empty;

        public string AwakeTime { get; set; } = string.Empty;
    }
}
