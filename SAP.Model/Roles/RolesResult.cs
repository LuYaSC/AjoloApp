using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Roles
{
    public class RolesResult : RolesDto
    {
        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
