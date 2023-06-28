using Microsoft.AspNetCore.Mvc;
using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Domain.Contract.Booking;

using BCMCH.OTM.API.ViewModels.Generic;
using BCMCH.OTM.API.ViewModels.ResponseMessage;
using Microsoft.AspNetCore.Authorization;
using BCMCH.OTM.API.Shared.General;
using Microsoft.Net.Http.Headers;

namespace BCMCH.OTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        #region PRIVATE
        private readonly IBookingDomainService _bookingService;
        #endregion

        #region CONSTRUCTOR
        public BookingController(IBookingDomainService bookingService)
        {
            _bookingService = bookingService;
        }
        #endregion

        #region PUBLIC

        [HttpGet]
        [Route("get-events")]
        public async Task<IActionResult> SelectAllEvents(string? fromDate="",string? toDate="")
        {
            try
            {
                var result = await _bookingService.GetBookingList(fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-events-forpathology-and-removable-devcices")]
        public async Task<IActionResult> GetBookingsForPathology(string? fromDate="",string? toDate="")
        {
            try
            {
                var result = await _bookingService.GetBookingsForPathology(fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        

        [HttpGet]
        [Route("get-events-sorted")]
        public async Task<IActionResult> SelectEventsSorted(bool PaginationEnabled=false, int pageNumber=0,string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            // @params
            // bool PaginationEnabled=false,
            // int pageNumber=0,
            // string? sortValue="",
            // string? sortType="",
            // string? fromDate="",
            // string? toDate=""

            try
            {
                var result = await _bookingService.GetBookingsSorted(PaginationEnabled, pageNumber, sortValue, sortType, fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-events-exported")]
        public async Task<IActionResult> ExportEvents( string? sortValue="",string? sortType="",string fromDate="",string toDate="")
        {
            Console.WriteLine();
            Console.Write("controller fromdate : ");
            Console.Write(fromDate);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("controller todate: ");
            Console.Write(toDate);
            Console.WriteLine();
            try
            {
                var result = await _bookingService.ExportEvents(sortValue, sortType, fromDate, toDate);
                return File(result, "application/octet-stream", "test.xlsx");
                // return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        



        [HttpGet]
        [Route("get-events-with-department")]
        public async Task<IActionResult> SelectEventsWithDepartment(string departmentIds ,string? fromDate="",string? toDate="")
        {
            try
            {
                var result = await _bookingService.GetBookingListWithDepartment(departmentIds,fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-events-with-otid")]
        public async Task<IActionResult> SelectEventsWithOtId(string otIds,string? fromDate="",string? toDate="")
        {
            try
            {
                // Task<IEnumerable<Bookings>> GetBookingListWithOtId(string otIds, string? fromDate,string? toDate)
                var result = await _bookingService.GetBookingListWithOtId(otIds, fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-events-and-allocation")]
        public async Task<IActionResult> SelectEventsAndAllocations(int departmentId=0,int operationTheatreId=0, string? fromDate="",string? toDate="")
        {
            try
            {
                var result = await _bookingService.SelectBookingsAndAllocations(departmentId, operationTheatreId ,  fromDate, toDate);
                return Ok(new ResponseVM<BookingsAndAllocations>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpGet]
        [Route("get-event-equipments-and-employees")]
        public async Task<IActionResult> SelectEventEquipmentsAndEmployees(int bookingId)
        {
            try
            {
                var result = await _bookingService.GetEventEquipmentsAndEmployees(bookingId);
                return Ok(new ResponseVM<EventFields>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-allocatedTheatres")]
        public async Task<IActionResult> SelectAllocatedTheatres(int departmentId=1, string? fromDate="",string? toDate="")
        {
            try
            {
                var result = await _bookingService.SelectAllocatedTheatres(departmentId ,  fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<int?>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        

        

        [HttpPost]
        [Route("add-booking")]
        public async Task<IActionResult> AddBooking(PostBookingModel booking)
        {
            try
            {
                var result = await _bookingService.AddBooking(booking);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        
        [HttpPost]
        [Route("add-waiting-list")]
        public async Task<IActionResult> AddWaitingList(PostBookingModel booking)
        {
            try
            {
                var result = await _bookingService.AddWaitingList(booking);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        #region OT-BLOCKING
        [HttpPost]
        [Route("add-blocking")]
        public async Task<IActionResult> AddBlocking(Blocking _blocking)
        {
            try
            {
                var result = await _bookingService.AddBlocking(_blocking);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("edit-blocking")]
        public async Task<IActionResult> EditBlocking(Blocking _blocking)
        {
            try
            {
                var result = await _bookingService.EditBlocking(_blocking);
                return Ok(result);
                // return Ok(new ResponseVM<IEnumerable<Blocking>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        #endregion





        [HttpPatch]
        [Route("update-booking")]
        public async Task<IActionResult> UpdateBooking(UpdateBookingModel _booking)
        {
            try
            {
                var result = await _bookingService.UpdateBooking(_booking);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpDelete]
        [Route("delete-booking")]
        public async Task<IActionResult> DeleteBooking(string IdArray="")
        {
            try
            {
                 
                var result = await _bookingService.DeleteBooking(IdArray);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        #endregion



        #region PATIENT-DATA
        [HttpGet]
        [Route("get-patient-data")]
        public async Task<IActionResult> GetPatient(string registrationNo)
        {
            try
            {
                var result = await _bookingService.GetPatientData(registrationNo);
                return Ok(new ResponseVM<IEnumerable<Patient>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        #endregion


        #region PATHOLOGY-SECTION
        [HttpGet]
        [Route("get-pathology")]
        public async Task<IActionResult> GetPathology(string startDate, string endDate)
        {
            try
            {
                var result = await _bookingService.GetPathology(startDate, endDate);
                return Ok(new ResponseVM<IEnumerable<Pathology>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpGet]
        [Route("get-pathology-samples")]
        public async Task<IActionResult> GetPathologySamples(int id )
        {
            try
            {
                var result = await _bookingService.GetPathologyDataWithId(id);
                return Ok(new ResponseVM<IEnumerable<PathologySample>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-pathology-samples-summary")]
        public async Task<IActionResult> GetPathologySummaryWithOperationId(int operationId )
        {
            try
            {
                var result = await _bookingService.GetPathologySummaryWithOperationId(operationId);
                return Ok(new ResponseVM<PathologySampleSummary>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        

        [HttpPost]
        [Route("post-pathology")]
        public async Task<IActionResult> PostPathology( Pathology Pathology )
        {
            try
            {
                // Task<IEnumerable<Pathology>> PostPathology(Pathology Pathology);
                var result = await _bookingService.PostPathology(Pathology);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpPost]
        [Route("edit-pathology")]
        public async Task<IActionResult> PatchPathology( Pathology pathology )
        {
            try
            {
                var result = await _bookingService.PatchPathology(pathology);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpDelete]
        [Route("delete-pathology")]
        public async Task<IActionResult> DeletePathology(string idArray )
        {
            try
            {
                var result = await _bookingService.DeletePathology(idArray);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        #endregion


        #region REMOVABLE_DEVICES
        [HttpGet]
        [Route("get-removable-devices-main")]
        public async Task<IActionResult> PostRemovableDevices()
        {
            try
            {
                var result = await _bookingService.GetRemovableDevices();
                return Ok(new ResponseVM<IEnumerable<RemovableDevicesMain>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-removable-devices-selected")]
        public async Task<IActionResult> GetRemovableDevicesSelected(int id )
        {
            // public async Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevicesSelected(int id)
            try
            {
                var result = await _bookingService.GetRemovableDevicesSelected(id);
                return Ok(new ResponseVM<IEnumerable<RemovableDevicesSelcted>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-removable-devices")]
        public async Task<IActionResult> GetRemovableDevicesWithDate(string start, string end)
        {
            try
            {
                var result = await _bookingService.GetRemovableDevicesWithDate(start,end);
                return Ok(new ResponseVM<IEnumerable<RemovableDevices>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-removable-devices-summary")]
        public async Task<IActionResult> GetRemovableDevicesSummaryWithId(int operationId )
        {
            try
            {
                var result = await _bookingService.GetRemovableDevicesSummaryWithId(operationId);
                return Ok(new ResponseVM<RemovableDeviceSummary>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpPost]
        [Route("post-removable-devices-form")]
        public async Task<IActionResult> PostRemovableDevices( RemovableDevicesMain removableDevicesMain )
        {
            try
            {
                // Task<IEnumerable<Pathology>> PostPathology(Pathology Pathology);
                var result = await _bookingService.PostRemovableDevices(removableDevicesMain);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpDelete]
        [Route("delete-removable-device-main")]
        public async Task<IActionResult> DeleteRemovableDeviceMain(string idArray )
        {
            try
            {
                var result = await _bookingService.DeleteRemovableDeviceMain(idArray);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // public async Task<IEnumerable<int>> DeleteRemovableDeviceMain(String idArray)

        [HttpPost]
        [Route("edit-removable-devices-form")]
        public async Task<IActionResult> EditRemovableDevices( RemovableDevicesMain removableDevicesMain )
        {
            try
            {
                // Task<IEnumerable<Pathology>> PostPathology(Pathology Pathology);
                var result = await _bookingService.EditRemovableDevices(removableDevicesMain);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        #endregion



        #region NON OP
        [HttpPost]
        [Route("add-non-op-request")]
        public async Task<IActionResult> AddNonOPRequest(NonOP nonOP)
        {
            try
            {
                var result = await _bookingService.AddNonOPRequest(nonOP);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [Route("get-non-op-requests")]
        public async Task<IActionResult> GetNonOPRequests(string start,string end)
        {

            try
            {
                var result = await _bookingService.GetNonOPRequests(start, end);
                // Task<IEnumerable<NonOP>> GetNonOPRequests(string start, string end);
                return Ok(new ResponseVM<IEnumerable<NonOP>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [Route("get-non-op-requests-summary")]
        public async Task<IActionResult> GetNonOPRequestsummaryOperationId(int operationId)
        {
            try
            {
                var result = await _bookingService.GetNonOPRequestsummaryOperationId(operationId);
                return Ok(new ResponseVM<IEnumerable<NonOP>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpPost]
        [Route("edit-non-op-requests")]
        public async Task<IActionResult> EditBlocking(NonOP nonOP)
        {
            try
            {
                var result = await _bookingService.EditNonOPRequests(nonOP);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpDelete]
        [Route("delete-non-op-requests")]
        public async Task<IActionResult>DeleteNonOPRequests(string idArray)
        {
            try
            {
                var result = await _bookingService.DeleteNonOPRequests(idArray);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        
        #endregion

        // update timeing 

        [HttpPatch]
        [Route("update-booking-timing")]
        public async Task<IActionResult> PostOTTimings(BookingTime bookingTime)
        {
            try
            {
                var result = await _bookingService.PostOTTimings(bookingTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // public async Task<IEnumerable<BookingTime>> PostOTTimings(BookingTime bookingTime)


        // EXPORT START
        [HttpGet]
        [Route("get-allocations-exported")]
        public async Task<IActionResult> ExportAllocations( string? sortValue="",string? sortType="",string fromDate="",string toDate="")
        {
            Console.WriteLine();
            Console.Write("controller fromdate : ");
            Console.Write(fromDate);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("controller todate: ");
            Console.Write(toDate);
            Console.WriteLine();
            try
            {
                var result = await _bookingService.ExportAllocation(sortValue, sortType, fromDate, toDate);
                return File(result, "application/octet-stream", "test.xlsx");
                // return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-pathology-exported")]
        public async Task<IActionResult> ExportPathology(string startDate,string endDate)
        {
            try
            {
                var result = await _bookingService.ExportPathology(startDate, endDate);
                return File(result, "application/octet-stream", "test.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-nonop-exported")]
        public async Task<IActionResult> ExportNonOperativeProcedure(string start,string end)
        {
            try
            {
                var result = await _bookingService.ExportNonOperativeProcedure(start,end);
                return File(result, "application/octet-stream", "test.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // EXPORT END

        // #################
        // DASHBOARD SECTION 
        [HttpGet]
        [Route("get-dashboard-ot-todays")]
        public async Task<IActionResult> GetTodaysOtStatuses()
        {
            try
            {
                var result = await _bookingService.GetTodaysOtStatuses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        // public async Task<IEnumerable<DashboardOperationTheatreStatus>> GetTodaysOtStatuses()

    }
}
