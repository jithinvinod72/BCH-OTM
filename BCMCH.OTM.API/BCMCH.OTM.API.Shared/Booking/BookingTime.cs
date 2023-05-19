using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class BookingTime
    {
        public int? BookingId {get;set;}
        public string? OtComplexEntry {get;set;}
        public string? PreOpEntryTime {get;set;}
        public string? PreOpExitTime {get;set;}
        public string? OtEntryTime {get;set;}
        public string? OtExitTime {get;set;}
        public string? PostOpEntryTime {get;set;}
        public string? PostOpExitTime {get;set;}
        public string? AverageSurgeryTime {get;set;}
        
    }
}

