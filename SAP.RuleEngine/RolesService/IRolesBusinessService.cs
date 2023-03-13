using SAP.Core.Business;
using SAP.Model.Roles;
using SAP.Model.TypeBusiness;
using SAP.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.TypeBusinessService
{
    public interface IRolesBusinessService
    {
        Result<RolesResult> GetById(GetRolesDto dto);

        Result<List<RolesResult>> GetAll();

        Result<string> Update(RolesDto dto);

        Result<string> Create(RolesDto dto);
    }
}
