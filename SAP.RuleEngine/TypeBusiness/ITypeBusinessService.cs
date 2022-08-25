using SAP.Core.Business;
using SAP.Model.TypeBusiness;
using SAP.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.TypeBusinessService
{
    public interface ITypeBusinessService<T> where T : BaseType
    {
        Result<GetTypeResult> GetById(GetTypeByIdDto dto);

        Result<List<GetTypeResult>> GetAll();

        Result<string> Update(GetTypeByIdDto dto);

        Result<string> Create(CreateTypeDto dto);
    }
}
