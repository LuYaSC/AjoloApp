using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.AssignationTutor
{
    public class CreateAssignedTutorDto
    {
        public int KidId { get; set; }

        public int ParentId { get; set; }

        public int RelationshipId { get; set; }

        public string Observations { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
