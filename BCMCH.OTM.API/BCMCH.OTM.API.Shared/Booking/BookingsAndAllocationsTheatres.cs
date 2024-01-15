using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.API.Shared.Booking
{
    public class BookingsAndAllocationsTheatres
    {
        public IEnumerable<Bookings> Bookings {get;set;}
        public IEnumerable<GetAllocation> Allocations {get;set;}
        public IEnumerable<int?>? Theatres {get;set;}
        public int? SelectedTheatre {get;set;}
    }
    
}