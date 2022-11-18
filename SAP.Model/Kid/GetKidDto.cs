using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Kid
{
    public class GetKidDto
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public int SexTypeId { get; set; }

        public DateTime? BornDate { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
