﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;

namespace BCMCH.OTM.Data.Contract.Booking
{
    public interface IBookingDataAccess
    {
        Task<IEnumerable<Bookings>> GetBookingList(int operationTheatreId, string? fromDate,string? toDate);
        Task<IEnumerable<PostBookingModel>> AddBooking(PostBookingModel booking);
        Task<IEnumerable<UpdateBookingModel>> UpdateBooking(UpdateBookingModel booking);


        // Status Check 
        Task<int> IsOperationTheatreAllocated(int operationTheatreId,int departmentId , string startDate, string endDate);
        Task<int> IsOperationTheatreBloked(int operationTheatreId, string startDate, string endDate);
        Task<int> IsOperationTheatreBooked(int bookingIdToExcludeFromSearch, int operationTheatreId, string startDate, string endDate);
        // Status Check 


        // OT blocking
        Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking);
        // OT blocking
    }
}