using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class KidLanguageBackground : KidBackgroundBase
    {
        public string PronounceSyllablesAt{ get; set; } = string.Empty;

        public string DescribeSyllables { get; set; } = string.Empty;

        public string WordWithCorrectArticulationAt { get; set; } = string.Empty;

        public string DescribeWordArticulation { get; set; } = string.Empty;
    }
}
