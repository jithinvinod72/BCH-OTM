﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.Data.Contract.Booking
{
    public interface IBookingDataAccess
    {
        Task<IEnumerable<Bookings>> GetBookingList(int departmentId , string? fromDate,string? toDate);
        Task<IEnumerable<Equipments>> GetEventEquipments(int bookingId);
        Task<IEnumerable<Employee>> GetEventEmployees(int bookingId);
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<GetAllocationModel>> GetAllocation(int departmentId, string? startDate, string? endDate);
        Task<IEnumerable<int>> AddBooking(PostBookingModel booking);
        Task<IEnumerable<UpdateBookingModel>> UpdateBooking(UpdateBookingModel booking);
        Task<IEnumerable<Bookings>> DeleteBooking(string IdArray);


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
