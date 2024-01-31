using AjoloApp.Core.Business;
using AjoloApp.Model.Parent;

namespace AjoloApp.RuleEngine.ParentService
{
    public interface IParentService
    {
        Result<ParentsResult> GetParentById(ParentByIdDto dto);

        Result<List<ParentsResult>> GetAllParents();

        Result<ParentsResult> GetParent(GetParentDto dto);

        Result<string> CreateParent(CreateParentDto dto);

        Result<string> UpdateParent(UpdateParentDto dto);

        Result<string> ActivateOrDeactivate(DeleteDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
