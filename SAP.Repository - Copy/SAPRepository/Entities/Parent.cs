using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Parent : BaseData
    {
        public Parent()
        {
            this.Kids = new HashSet<Kid>();
        }
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public virtual ICollection<Kid> Kids { get; set; }
    }
}
