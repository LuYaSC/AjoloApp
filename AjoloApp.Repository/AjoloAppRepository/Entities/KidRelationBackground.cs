using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
{
    public class KidRelationBackground : KidBackgroundBase
    {
        public string RelationWithParents { get; set; } = string.Empty;

        public string RelationWithMother { get; set; } = string.Empty;

        public  bool MotherPlays { get; set; }

        public string TimeOfDayPlayMother { get; set; } = string.Empty;

        public string RelationWithFather { get; set; } = string.Empty;

        public string TimeOfDayPlayFather { get; set; } = string.Empty;

        public string RelationWithSiblings { get; set; } = string.Empty;

        public string RelationParentsWithSiblings { get; set; } = string.Empty;

        public string RelationBetweenParentsAndChildren { get; set; } = string.Empty;

        public string RelationBetweenParentsAndGrandParents { get; set; } = string.Empty;

        public bool MakeFriendsEasily { get; set; }

        public string PrefferOlderOrYounger { get; set; } = string.Empty;

        public bool ShareWithOtherChildren { get; set; }

        public bool GetAlongWellWithAdults { get; set; }

        public bool IsCheerful { get; set; }

        public bool IsAgresive { get; set; }

        public string HowShowBehavior { get; set; } = string.Empty;

        public bool IsIndependent { get; set; }

        public bool CriesForNoReason { get; set; }

        public string HowItReactsWhencontracted { get; set; } = string.Empty;

        public bool TakeCareYourToys { get; set; }

        public string WhatKindOfToysYouLike { get; set; } = string.Empty;

        public string HowIsCorrect { get; set; } = string.Empty;

        public string WhenIsGoodWhatDoYouDo { get; set; } = string.Empty;
    }
}
