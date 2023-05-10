using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class PathologySample
    {
        public int? Id {get;set;}
        public int? ProcedureId {get;set;}
        public int? HistopathologyId {get;set;}
        public string? SpecimenNature {get;set;}
        public string? BiposySite {get;set;}
    }
}

