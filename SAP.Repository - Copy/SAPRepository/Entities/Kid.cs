using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Kid : BaseData
    {
        public Kid()
        {
            this.Parents = new HashSet<Parent>();
        }

        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public string Sex { get; set; }

        public DateTime? BornDate { get; set; }

        public DateTime AdmissionDate { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Parent> Parents { get; set; }
    }
}
