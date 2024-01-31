using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationTutor;

namespace AjoloApp.RuleEngine.AssignationTutorService
{
    public interface IAssignationTutorService
    {
        Result<string> DisableOrEnable(UpdateAssignedTutorDto dto);

        Result<List<AssignationTutorResult>> GetFilter(CreateAssignedTutorDto dto);

        Result<List<AssignationTutorResult>> GetAll();

        Result<string> Create(CreateAssignedTutorDto dto);

        Result<string> Update(UpdateAssignedTutorDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
