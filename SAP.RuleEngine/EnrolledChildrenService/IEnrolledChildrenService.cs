using SAP.Core.Business;
using SAP.Model.EnrolledChildren;

namespace SAP.RuleEngine.EnrolledChildrenService
{
    public interface IEnrolledChildrenService
    {
        Result<List<EnrolledChildrenResult>> GetAll();

        Result<string> Create(List<CreateEnrolledChildrenDto> dto);

        Result<string> Update(List<UpdateEnrolledChildrenDto> dto);
    }
}
