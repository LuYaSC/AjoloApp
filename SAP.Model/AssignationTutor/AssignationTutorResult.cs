using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.AssignationTutor
{
    public class AssignationTutorResult
    {
        public int Id { get; set; }

        public string KidName { get; set; }

        public int ParentName { get; set; }

        public int Relation { get; set; }

        public string Observations { get; set; }

        public bool IsAuthorized { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
