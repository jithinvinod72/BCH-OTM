using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class PathologySampleSummary
    {
        public IEnumerable<Pathology> pathology {get;set;}
        public IEnumerable<PathologySample> samples {get;set;}
    }
}

