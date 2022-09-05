using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.EnrolledChildren
{
    public class CreateEnrolledChildrenDto
    {
        public int AssignedTutorId { get; set; }

        public int AssignedRoomId { get; set; }

        public string Observations { get; set; }
    }
}
