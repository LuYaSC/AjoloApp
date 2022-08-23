using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Base
{
    public interface ILogicalDelete
    {
        bool IsDeleted { get; set; }
    }
}
