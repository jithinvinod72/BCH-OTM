using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class DashboardData
    {
        public IEnumerable<DashbordOTGroup> OtStatuses {get;set;}
        public IEnumerable<DashboardDepartmentGroups> DepartmentsStatuses {get;set;}
        
        
    }
    
}


