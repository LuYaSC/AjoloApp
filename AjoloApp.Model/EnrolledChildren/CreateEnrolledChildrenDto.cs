using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.EnrolledChildren
{
    public class CreateEnrolledChildrenDto
    {
        public int KidId { get; set; }

        public int AssignedTutorId { get; set; }

        public int AssignedRoomId { get; set; }

        public string Observations { get; set; }

        public decimal Amount { get; set; }

        public bool GeneratePayments { get; set; }
    }
}
