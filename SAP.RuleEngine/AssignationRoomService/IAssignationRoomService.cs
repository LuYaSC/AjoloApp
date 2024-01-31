using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationRoom;

namespace AjoloApp.RuleEngine.AssignationRoomService
{
    public interface IAssignationRoomService
    {
        Result<List<AssignationRoomResult>> GetFilter(CreateAssignedRoomDto dto);

        Result<List<AssignationRoomResult>> GetAll();

        Result<string> Create(CreateAssignedRoomDto dto);

        Result<string> Update(UpdateAssignedRoomDto dto);

        Result<string> DisableOrEnable(UpdateAssignedRoomDto dto);

        Result<AssignationRoomDetailResult> GetDetail(AssignationRoomDetailDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
