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
        Task<IEnumerable<Bookings>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate);
        Task<IEnumerable<PostBookingModel>> PostBooking(PostBookingModel _booking);
        Task<IEnumerable<PostBookingModel>> UpdateBooking(PostBookingModel _booking);


        // Status Check 
        Task<int> IsOperationTheatreAllocated(int _operationTheatreId,int _departmentId , DateTime _startDate, DateTime _endDate);
        Task<int> IsOperationTheatreBloked(int _operationTheatreId, DateTime _startDate, DateTime _endDate);
        Task<int> IsOperationTheatreBooked(int _operationTheatreId, DateTime _startDate, DateTime _endDate);
        // Status Check 


        // OT blocking
        Task<IEnumerable<Blocking>> PostBlocking(Blocking _blocking);
        // OT blocking
    }
}
