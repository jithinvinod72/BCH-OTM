using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class PathologySample
    {
        public int? Id {get;set;}
        public string RegistrationNo {get; set; }
        public string NestedData {get;set;}
        public int Status {get;set;}
        public bool IsDeleted {get;set;}

    }
}

