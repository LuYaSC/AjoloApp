using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Base
{
    public interface ILogicalDelete
    {
        bool IsDeleted { get; set; }
    }
}
