using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using System.Runtime.CompilerServices;
using System.Data.Common;
using BCMCH.OTM.API.Shared.General;
using System.Globalization;
using BCMCH.OTM.Infrastucture.Generic;

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


        public async Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel _booking)
        {
            
            
            #region VALIDATION
            // START - VALIDATION SECTION   
            var _OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(_OTAllocationStatus<1)
            {
               return new Envelope<IEnumerable<UpdateBookingModel>>(false,"data-update-failed");
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


            var result = await _bookingDataAccess.UpdateBooking(_booking);

             return new Envelope<IEnumerable<UpdateBookingModel>>(true,"data-update-success", result); ;
        }

        public async Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking)
        {
            var result = await _bookingDataAccess.AddBlocking(blocking);
            return result;
        }

        private DateTime convertTimeTwelveToTwentyFour( string _datetime )
        {
             DateTime dateTime =  DateTime.ParseExact( _datetime,  
                                                        "yyyy/MM/dd hh:mm:ss tt", 
                                                        System.Globalization.CultureInfo.InvariantCulture);
            Console.WriteLine();
            Console.Write("12 hour datetime : ");
            Console.Write(_datetime);
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("24 hour datetime : ");
            Console.Write(dateTime);
            Console.WriteLine();

            
            return dateTime;
        }
    }
}
