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
    public class EventFields
    {
        public IEnumerable<Departments>? Departments {get;set;}
        public IEnumerable<Employee>? Surgeons {get;set;}
        public IEnumerable<Employee>? Nurses {get;set;}
        public IEnumerable<Equipments>? Equipments {get;set;}   
        public IEnumerable<Equipments>? Medicines {get;set;}   
        public IEnumerable<Equipments>? Materials {get;set;}   
    }
    
}
