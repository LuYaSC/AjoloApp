using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.AssignationTutor
{
    public class AssignationTutorResult
    {
        public int Id { get; set; }

        public int KidId { get; set; }

        public string KidName { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public string Relation { get; set; }

        public string Observations { get; set; }

        public bool IsAuthorized { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
