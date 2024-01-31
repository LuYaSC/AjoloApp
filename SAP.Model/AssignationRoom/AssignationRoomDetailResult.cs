using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.AssignationRoom
{
    public class AssignationRoomDetailResult
    {
        public string Collaborator { get; set; }

        public string CollaboratorSex { get; set; }

        public string CollaboratorCity { get; set; }

        public string CollaboratorBranchOffice { get; set; }

        public string CollaboratorEmail { get; set; }

        public DateTime CollaboratorStartDate { get; set; }

        public string Room { get; set; }

        public string BranchOffice { get; set; }

        public string Modality { get; set; }

        public string City { get; set; }
    }
}
