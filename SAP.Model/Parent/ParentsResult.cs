using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAP.Model.Parent
{
    public class ParentsResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string? PlaceBorn { get; set; }

        public string BloodType { get; set; }

        public string DocumentNumber { get; set; }

        public string Sex { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
