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
        public string? Datetime {get;set;}
        public string? PatientFirstName {get;set;}
        public string? PatientMiddleName {get;set;}
        public string? PatientLastName {get;set;}
        public string? BookedByName {get;set;}
        public int? BookedDepartment {get;set;}
        public string? DepartmentName {get;set;}
        public string NestedData {get;set;}        
        public string? PostedBy {get;set;}
        public int?   Status {get;set;}
        public bool? IsDeleted {get;set;}

    }
}

