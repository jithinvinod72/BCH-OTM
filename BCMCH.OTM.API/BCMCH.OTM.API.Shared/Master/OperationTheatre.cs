
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class OperationTheatre
    {
        public int OperationTheatreId { get; set; }
        public string? Name { get; set; }
        public string? shortName { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public int ReservedDepartment { get; set; }
        public float CleaningTime { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

        