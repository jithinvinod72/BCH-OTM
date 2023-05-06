using System;
namespace BCMCH.OTM.API.Shared.Master
{
	public class NonOP
	{
        public int? Id { get; set; }
        public string PatientUHID { get; set; }
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public string Sex { get; set; }
        public string AdmittedLocation { get; set; }
        public int ProcedureToPerform { get; set; }
        public int PriorityLevel { get; set; }
        public string ProvisionalDiagnosis { get; set; }
        public string Comments { get; set; }
        public DateTime DateToBePerformed { get; set; }

    }
}

