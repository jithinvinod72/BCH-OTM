using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class OperationTheatreAllocation
    {
        public int Id {get; set;}
        public int OperationTheatreId {get; set;}
        public int AssignedDepartmentId {get; set;}
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}
        public string? ModifiedBy {get; set;}
    }
}

