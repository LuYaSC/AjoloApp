using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationRoom;
using AjoloApp.Model.EnrolledChildren;

namespace AjoloApp.RuleEngine.EnrolledChildrenService
{
    public interface IEnrolledChildrenService
    {
        Result<List<EnrolledChildrenResult>> GetFilter(EnrollFilterDto dto);

        Result<List<EnrolledChildrenResult>> GetAll();

        Result<EnrollChildrenDetailResult> GetDetail(UpdateAssignedRoomDto dto);

        Result<string> Create(CreateEnrolledChildrenDto dto);

        Result<string> Update(UpdateEnrolledChildrenDto dto);

        Result<string> ActivateOrDeactivate(UpdateEnrolledChildrenDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
