using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.Payment
{
    public class PaymentDetailResult
    {
        //Parent Data
        public string ParentName { get; set; }

        public string ParentRelation { get; set; }

        public string ParentMaritalStatus { get; set; }

        public string ParentDocumentType { get; set; }

        public string ParentDocumentNumber { get; set; }

        public string ParentPhoneNumber { get; set; }

        //Room Data
        public string CollaboratorName { get; set; }

        public string Room { get; set; }

        public string BranchOffice { get; set; }

        public string City { get; set; }

        public string Turn { get; set; }

        public string Modality { get; set; }

        public DateTime StartDateKid { get; set; }

        public string Antiquity { get; set; }
    }
}
