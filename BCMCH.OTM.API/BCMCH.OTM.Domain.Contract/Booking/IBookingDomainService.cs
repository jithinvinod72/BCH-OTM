using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Infrastucture.Generic;
using OfficeOpenXml;

namespace BCMCH.OTM.Domain.Contract.Booking
{
    public interface IBookingDomainService
    {
        Task<IEnumerable<Bookings>> GetBookingList(string fromDate, string toDate);
        Task<IEnumerable<Bookings>> GetBookingsForPathology(string? fromDate, string? toDate);
        Task<IEnumerable<Bookings>> GetBookingListWithDepartment(string departmentIds, string? fromDate,string? toDate);
        Task<IEnumerable<Bookings>> GetBookingListWithOtId(string otIds, string? fromDate,string? toDate);
        Task<IEnumerable<Bookings>> GetBookingsSorted(bool PaginationEnabled=false, int pageNumber=0,string? sortValue="",string? sortType="",string? fromDate="",string? toDate="");
        Task<Stream> ExportEvents( string? sortValue="",string? sortType="",string? fromDate="",string? toDate="");
        Task<EventFields> GetEventEquipmentsAndEmployees(int bookingId);
        Task<IEnumerable<int?>> SelectAllocatedTheatres(int departmentId, string? fromDate,string? toDate);
        Task<BookingsAndAllocations> SelectBookingsAndAllocations(int departmentId,int operationTheatreId , string? fromDate,string? toDate);
        Task<Envelope<IEnumerable<int>>> AddBooking(PostBookingModel booking);
        Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel _booking);
        Task<IEnumerable<Bookings>> DeleteBooking(string IdArray="");
        
        // Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking);
        Task<Envelope<IEnumerable<Blocking>>> AddBlocking(Blocking blocking);
        Task<Envelope<IEnumerable<Blocking>>> EditBlocking(Blocking blocking);

        Task<Envelope<IEnumerable<int>>> AddWaitingList(PostBookingModel booking);
        Task<IEnumerable<Patient>> GetPatientData(string registrationNo);

        // Pathology smaple
        Task<IEnumerable<Pathology>> GetPathology();
        Task<IEnumerable<PathologySample>> GetPathologyDataWithId(int id);
        Task<PathologySampleSummary> GetPathologySummaryWithOperationId(int operationId);
        Task<IEnumerable<int>> PostPathology(Pathology Pathology);
        Task<IEnumerable<int>> PatchPathology(Pathology pathology);
        Task<IEnumerable<int>> DeletePathology(string idArray);

        // Removable Devices
        Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevices();
        Task<IEnumerable<RemovableDevicesSelcted>> GetRemovableDevicesSelected(int id);
        Task<RemovableDeviceSummary> GetRemovableDevicesSummaryWithId(int operationId);
        Task<IEnumerable<int>> DeleteRemovableDeviceMain(string idArray);
        Task<IEnumerable<int>> PostRemovableDevices(RemovableDevicesMain removableDevicesMain);
        Task<IEnumerable<int>> EditRemovableDevices(RemovableDevicesMain removableDevicesMain);



        // non op
        Task<Envelope<IEnumerable<NonOP>>> AddNonOPRequest(NonOP nonOP);
        Task<IEnumerable<NonOP>> GetNonOPRequestsummaryOperationId(int operationId);
        Task<IEnumerable<NonOP>> GetNonOPRequests(string start, string end);
        Task<Envelope<IEnumerable<NonOP>>> EditNonOPRequests(NonOP nonOP);
        Task<IEnumerable<NonOP>> DeleteNonOPRequests(string idArray);

        // time 
        Task<IEnumerable<BookingTime>> PostOTTimings(BookingTime bookingTime);

        // EXPORTING 
        Task<Stream> ExportAllocation( string? sortValue="",string? sortType="",string? fromDate="",string? toDate="");
        Task<Stream> ExportPathology();
        Task<Stream> ExportNonOperativeProcedure(string start,string end);

        // DASHBOARD SECTION 
        Task<IEnumerable<DashbordOTGroup>> GetTodaysOtStatuses();
    }
}
