using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class RemovableDevicesSelcted
    {
        public int? Id {get;set;}
        public int? RemovableDeviceMainId {get;set;}
        public int? RemovableDeviceId {get;set;}
        public string? RemovableDeviceName {get;set;}
        public string? Notes {get;set;}
        public string? PlacedIn {get;set;}
        public string? PlacedDate {get;set;}
        public string? DateToRemove {get;set;}
        public int? IsRemoved {get;set;}
        
    }
}

