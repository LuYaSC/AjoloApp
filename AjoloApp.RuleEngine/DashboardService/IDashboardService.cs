using AjoloApp.Core.Business;
using AjoloApp.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.RuleEngine.DashboardService
{
    public interface IDashboardService
    {
        Result<DashboardResult> GetData();
    }
}
