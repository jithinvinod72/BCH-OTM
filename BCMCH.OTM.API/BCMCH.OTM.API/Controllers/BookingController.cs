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
            // @params
            // bool PaginationEnabled=false,
            // int pageNumber=0,
            // string? sortValue="",
            // string? sortType="",
            // string? fromDate="",
            // string? toDate=""
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
        [Route("excel-test")]
        public async Task<IActionResult> ExportEventstest()
        {
            try
            {
                var result =await _bookingService.ExcelTest2();
                return File(result, "application/octet-stream", "test.xlsx");
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
                return Ok(new ResponseVM<IEnumerable<Blocking>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
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
                return Ok(new ResponseVM<IEnumerable<Blocking>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
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
    }
}
