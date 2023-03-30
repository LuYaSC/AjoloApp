using SAP.Core.Business;
using SAP.Model.Kid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.KidService
{
    public interface IKidService
    {
        Result<KidsResult> GetKidById(KidByIdDto dto);

        Result<List<KidsResult>> GetAllKids();

        Result<List<GetDetailKidResult>> GetDetailKid(GetDetailKidDto dto);

        Result<KidsResult> GetKid(GetKidDto dto);

        Result<string> CreateKid(CreateKidDto dto);

        Result<string> UpdateKid(UpdateKidDto dto);

        Result<string> ActivateOrDeactivate(KidByIdDto dto);
    }
}
