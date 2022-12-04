using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public async Task<IEnumerable<Bookings>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(_operationTheatreId, _fromDate, _toDate);
            return result;
        }
        #endregion

        public async Task<IEnumerable<PostBookingModel>> PostBooking(PostBookingModel _booking)
        {
            // START - VALIDATION SECTION   
            var _OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);

            if(_OTAllocationStatus<1){
                // PostBookingModel _data = new (
                //     InstructionToNurse ="error"
                // );
                // var _error = new {
                //             _OTAllocationStatus=0, 
                //             _OTBlockStatus=0,
                //             _OTBookingStatus=0,
                //             Message = "Hello" 
                //         };
                // return _error;
            }
            var _OTBlockStatus      = await _bookingDataAccess.IsOperationTheatreBloked(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);
            var _OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(_booking.OperationTheatreId, _booking.StartDate, _booking.EndDate);
            // END - VALIDATION SECTION




            var result = await _bookingDataAccess.PostBooking(_booking);
            return result;
        }

        // public async Task<int> IsOperationTheatreAllocated()
        // {
        //     var result = await _bookingDataAccess.IsOperationTheatreAllocated();
        //     return result;
        // }

        
        
    }
}
