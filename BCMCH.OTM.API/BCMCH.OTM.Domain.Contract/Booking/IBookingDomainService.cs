using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;

namespace BCMCH.OTM.Domain.Contract.Booking
{
    public interface IBookingDomainService
    {
        Task<IEnumerable<Bookings>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate);
    }
}
