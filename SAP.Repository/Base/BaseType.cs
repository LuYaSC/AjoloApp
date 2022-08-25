using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.Base
{
    public class BaseType : BaseTrace
    {
        public string Description { get; set; }

        public string? Initial { get; set; }
    }
}
