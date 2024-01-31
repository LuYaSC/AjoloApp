using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.Payment
{
    public class PaymentFilterDto
    {
        public int KidId { get; set; }

        public int RoomId { get; set; }

        public int BranchOfficeId { get; set; }

        public int PaymentOperationId { get; set; }
    }
}
