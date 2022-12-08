using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class PostBookingModel
    {
        // public int? Id { get; set; }
        public int OperationTheatreId {get; set; }
        public int DepartmentId {get; set; }
        public int DoctorId {get; set; }
        public int AnaesthetistId {get; set; }
        public int StatusId {get; set; }
        public int AnaesthesiaTypeId {get; set; }
        public int SurgeryId {get; set; }
        public string RegistrationNo {get; set; }
        public DateTime StartDate {get; set; }
        public DateTime EndDate {get; set; }
        public float Duration {get; set; }
        public string? InstructionToNurse {get; set; }
        public string? InstructionToAnaesthetist {get; set; }
        public string? InstructionToOperationTeatrePersons {get; set; }
        public string? RequestForSpecialMeterial {get; set; }
        public string? Type {get; set; }
        public string? EmployeeIdArray {get; set; }
        public string? EquipmentsIdArray {get; set; }
    }
}

