using SAP.Core.Business;
using SAP.Model.TypeBusiness;
using SAP.Repository.Base;

namespace SAP.RuleEngine.TypeBusinessService
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
