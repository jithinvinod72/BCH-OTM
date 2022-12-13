﻿using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;

using BCMCH.OTM.API.Shared.General;

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
        public async Task<IEnumerable<Bookings>> GetBookingList(int operationTheatreId, string? fromDate,string? toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(operationTheatreId, fromDate, toDate);
            return result;
        }
        #endregion


        public async Task<IEnumerable<PostBookingModel>> AddBooking(PostBookingModel booking)
        {

            // convertTimeTwelveToTwentyFour(booking.EndDate);
            #region VALIDATION
            // START - VALIDATION SECTION   
            var _OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(_OTAllocationStatus<1)
            {
                // return Ok(new ResponseVM<bool>(false, "bla"));
                throw new InvalidOperationException("the ot "
                                                    +booking.OperationTheatreId 
                                                    +" is not allocated");
            }


            var _OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(_OTBlockStatus>0)
            {
                throw new InvalidOperationException("the ot "
                                                    +booking.OperationTheatreId 
                                                    +" is blocked");
            }


            var _OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(0,booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            //  here the first argument is zero 
            if(_OTBookingStatus>0)
            {
                throw new InvalidOperationException("the ot "
                                                    +booking.OperationTheatreId 
                                                    +" is already booked for the slot "
                                                    +booking.StartDate+" to "+booking.EndDate);
            }

            // END - VALIDATION SECTION
            #endregion




            var result = await _bookingDataAccess.AddBooking(booking);
            return result;
        }


        public async Task<IEnumerable<UpdateBookingModel>> UpdateBooking(UpdateBookingModel booking)
        {
            
            
            #region VALIDATION
            // START - VALIDATION SECTION   
            var _OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(_OTAllocationStatus<1)
            {
                throw new InvalidOperationException("Operation Theatre "
                                                    +booking.OperationTheatreId 
                                                    +" is not allocated");
                // return Ok(ResponseModel<IEnumerable<UpdateBookingModel>>((false, ResponseMessage.DATA_NOT_FOUND) );
                // return ResponseModel<IEnumerable<UpdateBookingModel>>(true, "data not found" );

                // Exception e = new InvalidOperationException("This statement is the original exception message.");

                // return new ResponseModel<IEnumerable<UpdateBookingModel>>(true, ResponseMessage.DATA_NOT_FOUND, );
            }


            var _OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(_OTBlockStatus>0)
            {
                throw new InvalidOperationException("Operation Theatre"
                                                    +booking.OperationTheatreId 
                                                    +" is blocked");
            }

            var _OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(booking.Id, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(_OTBookingStatus>0)
            {
                throw new InvalidOperationException("Operation Theatre"
                                                    +booking.OperationTheatreId 
                                                    +" is already booked for the slot "
                                                    +booking.StartDate+" to "+booking.EndDate);
            }

            // END - VALIDATION SECTION
            #endregion


            var result = await _bookingDataAccess.UpdateBooking(booking);
            return result;
        }

        public async Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking)
        {
            var result = await _bookingDataAccess.AddBlocking(blocking);
            return result;
        }

    }
}
