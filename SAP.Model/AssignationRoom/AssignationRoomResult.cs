namespace AjoloApp.Model.AssignationRoom
{
    public class AssignationRoomResult
    {
        public int Id { get; set; }

        public int CollaboratorId { get; set; }

        public string Collaborator { get; set; }

        public int RoomId { get; set; }

        public string Room { get; set; }

        public int TurnId { get; set; }

        public string Turn { get; set; }

        public int ModalityId { get; set; }

        public string Modality { get; set; }

        public int BranchOfficeId { get; set; }

        public string BranchOffice { get; set; }

        public int CityId { get; set; }

        public string City { get; set; }

        public string UserCreation { get; set; }

        public string UserModification { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        public string Observations { get; set; }

        public bool IsDeleted { get; set; }
    }
}
