using AjoloApp.Core.Business;
using AjoloApp.Model.TypeBusiness;
using AjoloApp.Repository.Base;

namespace AjoloApp.RuleEngine.TypeBusinessService
{
    public interface ITypeBusinessService<T> where T : BaseType
    {
        Result<GetTypeResult> GetById(GetTypeByIdDto dto);

        Result<List<GetTypeResult>> GetAll();

        Result<string> Update(GetTypeByIdDto dto);

        Result<string> Create(CreateTypeDto dto);

        Result<ReportResult> GeneratePdf(string title);
    }
}
