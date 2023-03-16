using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class Bookings
    {
        public int event_id {get; set; }
        public int OperationTheatreId {get; set; }
        public int? BookedByEmployee {get; set; }
        public int? BookedByDepartment {get; set; }
        public string? DepartmentName {get; set; }
        public string? BookedByDoctorFirstName {get;set;}
        public string? BookedByDoctorLastName {get;set;}
        public string? BookedByDoctorMiddleName {get;set;}
        public int? BookedByDoctorDepartmentId {get;set;}
        public int? AnaesthetistId {get; set; }
        public int? AnaesthesiaTypeId {get; set; }
        public int? SurgeryId {get; set; }

        public string? SurgeryName {get;set;}
        public string? SurgeryPrintName {get;set;}
        public string? SurgeryAliasName {get;set;}
        public string? SurgeryInstructionsToPatient {get;set;}

        public string? PatientRegistrationNo {get; set; }
        public DateTime StartDate {get; set; }
        public DateTime EndDate {get; set; }
        public float? OperationDuration {get; set; }
        public string? InstructionToNurse {get; set; }
        public string? InstructionToAnaesthetist {get; set; }
        public string? InstructionToOperationTeatrePersons {get; set; }
        public string? RequestForSpecialMeterial {get; set; }
        public string? ModifiedBy {get; set; }
        public bool? IsDeleted {get; set; }
        public string? TheatreName {get; set; }
        public string? TheatreLocation {get; set; }
        public string? TheatreType {get; set; }
        public int? TheatreDefaultDepartment {get; set; }
        public float? TheatreCleaningTime  {get; set; }
        public string? AnaesthetistFirstName {get; set; }
        public string? AnaesthetistLastName {get; set; }
        public string? AnaesthetistMiddleName {get; set; }
        public int? AnaesthetistDepartmentId {get; set; }
        public string? AnaesthesiaType {get; set; }
        public int? StatusCode  {get; set; }
        public string? StatusName  {get; set; }

        public string? PatientFirstName   {get; set; }
        public string? PatientMiddleName {get;set;}
        public string? PatientLastName {get;set;}
        public string? PatientDateOfBirth {get;set;}
        public int? PatientGender {get;set;}
    }
    
}
