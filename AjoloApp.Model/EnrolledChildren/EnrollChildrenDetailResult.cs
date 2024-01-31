using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.EnrolledChildren
{
    public class EnrollChildrenDetailResult
    {
        public EnrollChildrenDetailResult()
        {
            Parents = new List<Parents>();
        }
        //Data for Kids
        public string KidName { get; set; }

        public int KidId { get; set; }

        public DateTime BornDateKid { get; set; }

        public int AssignedRoomId { get; set; }

        public string AgeKid { get; set; }

        public string BloodTypeKid { get; set; }

        public string SexKid { get; set; }

        public int QuantityTutors { get; set; }

        //Data for Parents
        public List<Parents> Parents { get; set; }

        //Data for Room
        public string Room { get; set; }

        public string City { get; set; }

        public string BranchOffice { get; set; }

        public string Turn { get; set; }

        public string Modality { get; set; }

        public string Schedule { get; set; }

        public string Observations { get; set; }

        public string Collaborator { get; set; }

        public DateTime StartDateKid { get; set; }

    }

    public class Parents
    {
        public string ParentName { get; set; }

        public string Relation { get; set; }

        public string PhoneNumber { get; set; }

        public string BloodTypeParent { get; set; }

        public string SexParent { get; set; }

        public string Address { get; set; }

        public bool IsAuthorized { get; set; }

        public string MaritalStatus { get; set; }
    }
}
