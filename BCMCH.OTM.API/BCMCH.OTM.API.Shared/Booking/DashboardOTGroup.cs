namespace BCMCH.OTM.API.Shared.Booking
{
    // Define the OperationTheatreGroup class
    public class DashbordOTGroup
    {
        public int? OperationTheatreId { get; set; }
        public string? OperationTheatreName { get; set; }
        public int? TotalCases { get; set; }
        public int? CompletedCases { get; set; }
        public int? InComplexCasesCount {get;set;}
        public int? InPreOpCasesCount {get;set;}
        public int? InPostOpCasesCount {get;set;}
        public int? inOtCasesCount {get;set;}
        public int? pendingCasesCount {get;set;}
        public List<DashboardOperation> OperationsList { get; set; }
    }    
}

