using Microsoft.AspNetCore.Mvc;
using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.Domain.Contract.Booking;

using BCMCH.OTM.API.ViewModels.Generic;
using BCMCH.OTM.API.ViewModels.ResponseMessage;


namespace BCMCH.OTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> SelectEvents(int departmentId=1, int operationTheatreId=0 ,string? fromDate="",string? toDate="")
        {
            try
            {
                // bla
                var result = await _bookingService.GetBookingList(departmentId, operationTheatreId, fromDate, toDate);
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



        #endregion




    }
}
