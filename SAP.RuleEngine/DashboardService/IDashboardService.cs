using SAP.Core.Business;
using SAP.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.RuleEngine.DashboardService
{
    public interface IDashboardService
    {
        Result<DashboardResult> GetData();
    }
}
