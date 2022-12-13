using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class Blocking
    {
        public int OperationTheatreId {get; set;}
        public string StartDate {get; set;}
        public string EndDate {get; set;}
        public float? Duration {get; set;}
        public string? ModifiedBy {get; set;}
        
    }
}
