using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.AssignationRoom
{
    public class CreateAssignedRoomDto
    {
        public int CollaboratorId { get; set; }

        public int RoomId { get; set; }

        public int TurnId { get; set; }

        public int ModalityId { get; set; }

        public int BranchOfficeId { get; set; }

        public int CityId { get; set; }

        public string Observations { get; set; }
    }
}
