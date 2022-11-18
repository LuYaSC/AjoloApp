using SAP.Model.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Collaborator
{
    public class CreateCollaboratorDto
    {
        public string Name { get; set; }
        public string FirstLastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime BornDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public int SexTypeId { get; set; }
        public int BloodTypeId { get; set; }
        public int BranchOfficeId { get; set; }
        public int CityId { get; set; }
        public string Email { get; set; }
        public List<int> Roles { get; set; }
    }
}
