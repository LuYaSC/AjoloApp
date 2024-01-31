using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.Payment
{
    public class UpdatePaymentDto : CreatePaymentDto
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
