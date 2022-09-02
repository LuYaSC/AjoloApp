using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Collaborator
{
    public class CollaboratorsResult
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public DateTime BornDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DocumentNumber { get; set; }

        public string DocumentType { get; set; }

        public string Sex { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public string UserAssigned { get; set; }

        public string BranchOfficeAssigned { get; set; }

        public string CityAssigned { get; set; }

        public List<string> Roles { get; set; }
    }
}
