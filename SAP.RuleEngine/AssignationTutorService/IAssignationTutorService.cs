using SAP.Core.Business;
using SAP.Model.AssignationTutor;

namespace SAP.RuleEngine.AssignationTutorService
{
    public interface IAssignationTutorService
    {
        Result<List<AssignationTutorResult>> GetAll();

        Result<string> Create(List<CreateAssignedTutorDto> dto);

        Result<string> Update(List<UpdateAssignedTutorDto> dto);
    }
}
