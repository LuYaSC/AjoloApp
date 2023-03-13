using SAP.Core.Business;
using SAP.Model.Collaborator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.CollaboratorService
{
    public interface ICollaboratorService
    {
        Result<List<CollaboratorsResult>> GetCollaborators();

        Result<CollaboratorsResult> GetCollaboratorById(CollaboratorByIdDto dto);

        Result<string> CreateCollaborator(CreateCollaboratorDto dto);

        Result<string> UpdateCollaborator(UpdateCollaboratorDto dto);
    }
}
