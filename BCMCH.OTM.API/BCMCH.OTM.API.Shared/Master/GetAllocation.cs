using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class GetAllocation
    {
        public int? Id {get; set; }
        public int? OperationTheatreId {get; set; }
        public int? AssignedDepartmentId {get; set; }
        public string? GroupId {get; set; }
        public string? AssignedDepartmentName {get; set; }
        public DateTime? StartDate {get; set; }
        public DateTime? EndDate {get; set; }
        public string? OperationTheatreName {get; set; }
    }
}
