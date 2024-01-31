using AjoloApp.Core.Business;
using AjoloApp.Model.Kid;

namespace AjoloApp.RuleEngine.KidService
{
    public interface IKidService
    {
        Result<KidsResult> GetKidById(KidByIdDto dto);

        Result<List<KidsResult>> GetAllKids();

        Result<List<GetDetailKidResult>> GetDetailKid(GetDetailKidDto dto);

        Result<KidsResult> GetKid(GetKidDto dto);

        Result<string> CreateKid(CreateKidDto dto);

        Result<string> UpdateKid(UpdateKidDto dto);

        Result<string> ActivateOrDeactivate(KidByIdDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
