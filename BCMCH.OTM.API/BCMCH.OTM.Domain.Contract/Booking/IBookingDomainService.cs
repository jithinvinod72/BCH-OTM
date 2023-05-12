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
        Task<IEnumerable<int>> PostPathology(Pathology Pathology);
        Task<IEnumerable<int>> PatchPathology(Pathology pathology);
        Task<IEnumerable<int>> DeletePathology(string idArray);

        // Removable Devices
        Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevices();
        Task<IEnumerable<int>> PostRemovableDevices(RemovableDevicesMain removableDevicesMain);
    }
}
