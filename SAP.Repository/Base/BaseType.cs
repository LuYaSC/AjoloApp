using AjoloApp.Repository.AjoloAppRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Repository.Base
{
    public class BaseType : BaseTrace
    {
        public string Description { get; set; }

        public string? Initial { get; set; }
    }
}
