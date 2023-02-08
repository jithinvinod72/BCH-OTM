namespace BCMCH.OTM.API.Shared.General
{
    public class Patient
    {
        public string RegistrationNo {get;set;} 
        public string? Title {get;set;} 
        public string? FirstName {get;set;} 
        public string? MiddleName {get;set;} 
        public string? LastName {get;set;} 
        public string? Contact {get;set;} 
        public string? EmailID {get;set;} 
        public string? DateOfBirth {get;set;} 
        public string? YearOfBirth {get;set;} 
        public int? Gender {get;set;} 
        public string? Address {get;set;} 
        public string? District {get;set;} 
        public int Active {get;set;} 
    }
}