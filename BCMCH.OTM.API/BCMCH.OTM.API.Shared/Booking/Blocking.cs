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
        public int StatusId {get; set;}
        public DateTime StartDate {get; set;}
        public DateTime EndDate {get; set;}
        public float? Duration {get; set;}
        public string? ModifiedBy {get; set;}
        public string? Type {get; set;}
        
    }
}
