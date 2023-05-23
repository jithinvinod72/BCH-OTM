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
    public class DashboardOperation
    {
        public int? operationId {get;set;}
        public int? OperationTheatreId {get;set;}
        public string? OperationTheatreName {get;set;}
        public string? PatientFirstName {get;set;}
        public string? PatientMiddleName {get;set;}
        public string? PatientLastName {get;set;}
        public string? RegistrationNo {get;set;}
        public string? StartDate {get;set;}
        public string? EndDate {get;set;}
        public string? DepartmentId {get;set;}
        public string? OtComplexEntry {get;set;}
        public string? PreOpEntryTime {get;set;}
        public string? PreOpExitTime {get;set;}
        public string? OtEntryTime {get;set;}
        public string? OtExitTime {get;set;}
        public string? PostOpEntryTime {get;set;}
        public string? PostOpExitTime {get;set;}
        public string? ComplexLocation {get;set;}
        public int? AverageSurgeryTime {get;set;}
        
    }
    
}
