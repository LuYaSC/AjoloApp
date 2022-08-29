using SAP.Core.Business;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.Security.Principal;

namespace SAP.RuleEngine.CollaboratorService
{
    public class CollaboratorService : BaseBusiness<Collaborator, SAPContext>, ICollaboratorService
    {
        public CollaboratorService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
        }


    }
}
