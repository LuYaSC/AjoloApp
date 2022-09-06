using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.AssignationRoomService
{
    public interface IAssignationRoomService
    {
        Result<List<AssignationRoomResult>> GetAll();

        Result<string> Create(List<CreateAssignedRoomDto> dto);

        Result<string> Update(List<UpdateAssignedRoomDto> dto);
    }
}
