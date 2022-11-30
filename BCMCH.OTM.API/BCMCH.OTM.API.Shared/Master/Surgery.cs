
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class Surgery
    {
        public int Id {get; set; }
        public string? SurgeryCode {get; set; }
        public string? Name {get; set; }
        public string? PrintName {get; set; }
        public string? AliasName {get; set; }
        public string? PrintSequence  {get; set; }
        public bool IsOutSourced {get; set; }
        public string? InstructionsToPatient {get; set; }
        public bool IsConfidential {get; set; }
        public bool AppointmentRequired {get; set; }
        public string? InstructionsForPreparation {get; set; }
        public bool ConsentRequired {get; set; }
        public bool RequestFormExist {get; set; }
        public int ProviderCode {get; set; }
        public int LocationCode  {get; set; }
        public bool AllowArbitraryRating {get; set; }
        public bool ApplySpecialCharge {get; set; }
        public int RevenueHeadCode {get; set; }
        public int CostCenter {get; set; }
        public bool AuthorisationRequired {get; set; }
        public string? Remarks {get; set; }
        public bool CanPrint {get; set; }
        public String CreatedBy {get; set; }
    }
}

        


// Id
// SurgeryCode
// Name
// PrintName
// AliasName
// PrintSequence 
// IsOutSourced
// InstructionsToPatient
// IsConfidential
// AppointmentRequired
// InstructionsForPreparation
// ConsentRequired
// RequestFormExist
// ProviderCode
// LocationCode 
// AllowArbitraryRating
// ApplySpecialCharge
// RevenueHeadCode
// CostCenter
// AuthorisationRequired
// Remarks
// CanPrint
// CreatedB