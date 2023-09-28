using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class RemovableDevices
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
        public int? MainId {get; set; }
        public int? OperationId {get; set; }
        public int? status {get; set; }
        public int? IsDeleted {get; set; }
        public string? PostedBy {get; set; }
        public string? DateTime {get; set; }
        public int? BookingId {get; set; }
        public string? RegistrationNo {get; set; }
        public string? PatientFirstName {get; set; }
        public string? PatientMiddleName {get; set; }
        public string? PatientLastName {get; set; }
        public int? BookedByDepartment {get; set; }
        public string? DepartmentName {get; set; }
        


    }
}
