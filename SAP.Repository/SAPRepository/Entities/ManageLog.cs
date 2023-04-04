using SAP.Repository.Base;
using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class ManageLog : BaseData
    {
        public string RequestMethod { get; set; }

        public string RequestUrl { get; set; }

        public string RequestBody { get; set; }

        public string RequestHeaders { get; set; }

        public DateTime RequestTime { get; set; }
        
    }
}
