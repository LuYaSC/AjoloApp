using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.TypeBusiness
{
    public class GetTypeResult
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
