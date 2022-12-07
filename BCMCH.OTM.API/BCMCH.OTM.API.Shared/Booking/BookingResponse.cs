using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using 

namespace BCMCH.OTM.API.Shared.Booking
{
    public class BookingResponse
    {
        public int BookingId {get; set; }
        public int OperationTheatreId {get; set; }
        public int? BookedByEmployee{get; set; }
        public int? BookedByDepartment{get; set; }
        public int? AnaesthetistId {get; set; }
        public int? BookingStatusId {get; set; }
        public int? AnaesthesiaTypeId {get; set; }
        public int? SurgeryId {get; set; }
        public string? PatientRegistrationNo {get; set; }
        public DateTime OperationStartDate {get; set; }
        public DateTime OperationEndDate {get; set; }
        public float OperationDuration {get; set; }
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
        public int? AnaesthetistPositionId {get; set; }
        public int? AnaesthetistJobId {get; set; }
        public int? AnaesthetistGender {get; set; }
        public string? AnaesthetistEmployeeCurrentStatus {get; set; }
        public bool AnaesthetistEmployeeIsActive {get; set; }
        public string? AnaesthesiaType {get; set; }
        public int? StatusCode  {get; set; }
        public string? StatusName  {get; set; }
        public List<Equipment?> EquipmentsMapping {get; set; }
        public List<Employee?> EmployeesMapping {get; set; }
    }
    
}

