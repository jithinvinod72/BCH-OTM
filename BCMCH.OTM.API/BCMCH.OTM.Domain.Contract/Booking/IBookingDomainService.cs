using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Infrastucture.Generic;

namespace BCMCH.OTM.Domain.Contract.Booking
{
    public interface IBookingDomainService
    {
        Task<IEnumerable<Bookings>> GetBookingList(int operationTheatreId, string? fromDate,string? toDate);
        Task<Envelope<IEnumerable<PostBookingModel>>> AddBooking(PostBookingModel booking);
        Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel _booking);
        Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking);

    }
}
