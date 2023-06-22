using System;
namespace BCMCH.OTM.API.Shared.Booking
{
	public class NonOP
	{
        public int? Id { get; set; }
        public string? PatientUHID { get; set; }
        public int OperationId { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientMiddleName { get; set; }
        public string? PatientLastName { get; set; }
        public int ProcedureToPerform { get; set; }
        public int PriorityLevel { get; set; }
        public string? ProvisionalDiagnosis { get; set; }
        public string? Comments { get; set; }
        public string? Status { get; set; }
        public DateTime DateToBePerformed { get; set; }
        public DateTime? PostedDateTime { get; set; }

    }
}

