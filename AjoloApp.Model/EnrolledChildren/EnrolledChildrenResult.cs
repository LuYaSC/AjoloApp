using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.EnrolledChildren
{
    public class EnrolledChildrenResult
    {
        public int Id { get; set; }

        public string Parent { get; set; }

        public int KidId { get; set; }

        public string Kid { get; set; }

        public int AssignedRoomId { get; set; }

        public bool IsAuthorized { get; set; }

        public string Collaborator { get; set; }

        public string Room { get; set; }

        public string Turn { get; set; }

        public string Modality { get; set; }

        public string BranchOffice { get; set; }

        public string Observations { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public bool IsDeleted { get; set; }
    }
}
