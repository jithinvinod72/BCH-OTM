using System;
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
        Task<IEnumerable<Bookings>> GetBookingList(string? fromDate,string? toDate);
        Task<IEnumerable<Bookings>> GetBookingsForPathology(string? fromDate, string? toDate);

        Task<IEnumerable<Equipments>> GetEventEquipments(int bookingId);
        Task<IEnumerable<Equipments>> GetEventMedicines(int bookingId);
        Task<IEnumerable<Equipments>> GetEventMaterials(int bookingId);

        Task<IEnumerable<Employee>> GetEventEmployees(int bookingId);
        Task<IEnumerable<Surgeries>> GetEventSurgeries(int bookingId);
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<GetAllocation>> GetAllocation(string? startDate, string? endDate);
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
        Task<IEnumerable<Blocking>> EditBlocking(Blocking blocking);
        // OT blocking

        Task<IEnumerable<Patient>> GetPatientData(string registrationNo);

        // PATHOLOGY SAMPLES
        Task<IEnumerable<Pathology>> GetPathology();
        Task<IEnumerable<PathologySample>> GetPathologyDataWithId(int id);
        Task<IEnumerable<int>> PostPathology(Pathology Pathology);
        Task<IEnumerable<int>> PatchPathology(Pathology pathology);
        Task<IEnumerable<int>> DeletePathology(String idArray);

        // Removable Devices
        Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevicesWithOperationId(int operationId);
        Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevices();
        Task<IEnumerable<RemovableDevicesSelcted>> GetRemovableDevicesSelected(int id);
        Task<IEnumerable<int>> DeleteRemovableDeviceMain(string idArray);
        Task<IEnumerable<int>> PostRemovableDevices(RemovableDevicesMain removableDevicesMain);
        Task<IEnumerable<int>> EditRemovableDevices(RemovableDevicesMain removableDevicesMain);



        //NonOp
        Task<IEnumerable<NonOP>> AddNonOPRequest(NonOP nonOP);
        Task<IEnumerable<NonOP>> GetNonOPRequests();
        Task<IEnumerable<NonOP>> EditNonOPRequests(NonOP nonOP);

        // time 
        Task<IEnumerable<BookingTime>> PostOTTimings(BookingTime bookingTime);

        // DASHBOARD 
        Task<IEnumerable<DashboardOperation>> GetTodaysOtStatuses();
    }
}
