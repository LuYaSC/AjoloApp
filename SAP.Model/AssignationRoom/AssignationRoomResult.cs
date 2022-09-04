using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.AssignationRoom
{
    public class AssignationRoomResult
    {
        public int Id { get; set; }

        public int Collaborator { get; set; }

        public int Room { get; set; }

        public int Turn { get; set; }

        public int Modality { get; set; }

        public int BranchOffice { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public string Observations { get; set; }
    }
}
