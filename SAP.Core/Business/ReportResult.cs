using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Core.Business
{
    public class ReportResult
    {
        public string ReportName { get; set; }

        public byte[] Report { get; set; }
    }
}
