using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.Dashboard
{
    public class DashboardResult
    {
        public DashboardResult()
        {
            Collaborators = new List<CollaboratorData>();
        }

        //firstSection
        public int QuantityRegisteredPayments { get; set; }

        public decimal TotalRegisteredPayments { get; set; }

        public int QuantityPayedPayments { get; set; }

        public decimal TotalPayedPayments { get; set; }

        public int QuantityPartiallyPayedPayments { get; set; }

        public decimal TotalPartiallyPayedPayments { get; set; }

        public int QuantityUnpayPayments { get; set; }

        public decimal TotalUnpayPayments { get; set; }

        //second section Chart for Inscriptions

        //paymentType Section

        public int QuantityCashPayments { get; set; }

        public decimal TotalCashPayments { get; set; }

        public int QuantityQrPayments { get; set; }

        public decimal TotalQrPayments { get; set; }

        public int QuantityTransferPayments { get; set; }

        public decimal TotalTransferPayments { get; set; }

        //third Section dates in general

        public int QuantityTotalChildren { get; set; }

        public int QuantityTotalParents { get; set; }

        public int QuantityTotalCollaborators { get; set; }

        public int QuantityTotalPayments { get; set; }

        public int QuantityTotalInscriptions { get; set; }

        //four Section Data for Collaborators
        public List<CollaboratorData> Collaborators { get; set; }


    }

    public class CollaboratorData
    {
        public string Collaborator { get; set; }

        public string CollaboratorAge { get; set; }

        public DateTime CollaboratorStartDate { get; set; }

        public int QuantityChildrenAssigned { get; set; }

        public string CollaboratorCity { get; set; }

        public string CollaboratorBranchOffice { get; set; }

        public bool IsDeleted { get; set; }
    }
}
