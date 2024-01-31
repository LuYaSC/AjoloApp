using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidFoodBackground : KidBackgroundBase
    {
        public bool WasBreastfed { get; set; }

        public string UntielWhenBreastfed { get; set; } = string.Empty;

        public bool MotherBreastfed { get; set; }

        public string OtherPersonBreastfed { get; set; } = string.Empty;

        public string SinceGiveSolidFood { get; set; } = string.Empty;

        public string DescribeSolidFood { get; set; } = string.Empty;

        public bool AcceptEasilySolidFood { get; set; }

        public string IntervalsSolidFood { get; set; } = string.Empty;

        public string CurrentlyEatGood { get; set; } = string.Empty;

        public string DescribeFoodEat { get; set; } = string.Empty;

        public bool ChooseFood { get; set; }

        public string DescribeChooseFood { get; set; } = string.Empty;

        public bool IsAllergicFood { get; set; }

        public string DescribeAllergicFood { get; set; } = string.Empty;

        public bool HasTendencyToVomit { get; set; }

        public bool HasTendencyToDiarrhea { get; set; }

        public string MedicationsInCaseFever { get; set; } = string.Empty;

        public string DosisInCaseFever { get; set; } = string.Empty;

        public string DescribeYourChildren { get; set; } = string.Empty;
    }
}
