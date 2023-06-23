using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class RemovableDeviceSummary
    {
        public IEnumerable<RemovableDevicesMain> RemovableDevicesMain {get;set;}
        public IEnumerable<RemovableDevicesSelcted> RemovableDevicesSelcted {get;set;}
        
    }
}

