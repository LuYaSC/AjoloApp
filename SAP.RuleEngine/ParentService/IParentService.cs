using SAP.Core.Business;
using SAP.Model.Parent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.ParentService
{
    public interface IParentService
    {
        Result<ParentsResult> GetParentById(ParentByIdDto dto);

        Result<List<ParentsResult>> GetAllParents();

        Result<ParentsResult> GetParent(GetParentDto dto);

        Result<string> CreateParent(CreateParentDto dto);

        Result<string> UpdateParent(UpdateParentDto dto);
    }
}
