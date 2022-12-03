using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;

namespace BCMCH.OTM.Domain.Booking
{
    public class BookingDomainService : IBookingDomainService
    {
        #region PRIVATE
        private readonly IBookingDataAccess _bookingDataAccess;
        #endregion

        #region CONSTRUCTOR
        public BookingDomainService(IBookingDataAccess bookingDataAccess)
        {
            _bookingDataAccess = bookingDataAccess;
        }
        #endregion

        #region PUBLIC
        public async  Task<IEnumerable<Bookings>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(_operationTheatreId, _fromDate, _toDate);
            return result;
        }
        #endregion
        
    }
}
