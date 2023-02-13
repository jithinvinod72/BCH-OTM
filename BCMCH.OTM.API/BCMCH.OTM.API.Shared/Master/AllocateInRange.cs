using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class AllocateInRange
    {
        public int? OperationTheatreId {get; set; }
        public int? AssignedDepartmentId {get; set; }
        public string StartDate {get; set; }
        public string EndDate {get; set; }
        public string StartTime {get; set; }
        public string EndTime {get; set; }
        public string day {get; set; }
        public int? ModifiedBy {get; set; }
    }
}
