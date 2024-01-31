using AjoloApp.Core.Business;
using AjoloApp.Model.Roles;
using AjoloApp.Model.TypeBusiness;
using AjoloApp.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.RuleEngine.TypeBusinessService
{
    public interface IRolesBusinessService
    {
        Result<RolesResult> GetById(GetRolesDto dto);

        Result<List<RolesResult>> GetAll();

        Result<string> Update(RolesDto dto);

        Result<string> Create(RolesDto dto);
    }
}
