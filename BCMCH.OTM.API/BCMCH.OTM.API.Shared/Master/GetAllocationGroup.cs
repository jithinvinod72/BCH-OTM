using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.API.Shared.Master
{
    public class GetAllocationGroupped
    {
        public string? GroupId {get;set;}
        public IEnumerable<GetAllocation> Allocations {get;set;}

        public static implicit operator List<object>(GetAllocationGroupped v)
        {
            throw new NotImplementedException();
        }
    }
}
