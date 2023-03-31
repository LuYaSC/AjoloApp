using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Kid
{
    public class GetDetailKidResult
    {
        //kid data
        public string KidName { get; set; }

        public string AgeKid { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        //parents data
        public string ParentName { get; set; }

        public string Relation { get; set; }

        public string MaritalStatus { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsAuthorized { get; set; }

        public bool IsPrincipal { get; set; }
    }
    
}
