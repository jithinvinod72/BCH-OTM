﻿using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using System.Runtime.CompilerServices;
using System.Data.Common;
using System.Globalization;
using BCMCH.OTM.Infrastucture.Generic;
using BCMCH.OTM.API.Shared.Master;

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


        public async Task<Envelope<IEnumerable<PostBookingModel>>> AddBooking(PostBookingModel booking)
        {

            // convertTimeTwelveToTwentyFour(booking.EndDate);
            #region VALIDATION
            // START - VALIDATION SECTION   
            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if (OTAllocationStatus < 1)
            {
                return new Envelope<IEnumerable<PostBookingModel>>(false, $"the ot "
                                                    + booking.OperationTheatreId
                                                    + " is not allocated");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<PostBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is blocked");
            }

            var OTBookingStatus = await _bookingDataAccess.IsOperationTheatreBooked(0, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBookingStatus > 0)
            {
                return new Envelope<IEnumerable<PostBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is already booked for the slot ${booking.StartDate} to ${booking.EndDate}");
            }


            // END - VALIDATION SECTION
            #endregion        


            var result = await _bookingDataAccess.AddBooking(booking);
            return new Envelope<IEnumerable<PostBookingModel>>(true, "booking created", result); ;
        }


        public async Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel booking)
        {
            
            
            #region VALIDATION  
            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(OTAllocationStatus < 1)
            {
               return new Envelope<IEnumerable<UpdateBookingModel>>(false,"data-update-failed");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is blocked");
            }

            var OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(booking.Id, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBookingStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"Operation Theatre {booking.OperationTheatreId} is already booked for the slot ${booking.StartDate} to ${booking.EndDate}");
            }
            #endregion


             var result = await _bookingDataAccess.UpdateBooking(booking);

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

        
        public async Task<BookingsAndAllocations> SelectBookingsAndAllocations(int operationTheatreId, string? fromDate,string? toDate)
        {
            var bookings = await _bookingDataAccess.GetBookingList(operationTheatreId, fromDate, toDate);
            var allocations = await _bookingDataAccess.GetAllocation(operationTheatreId, fromDate, toDate);
            var result = new BookingsAndAllocations();
            result.Bookings = bookings;
            result.Allocations = allocations;

            return result;
        }
    }
}
