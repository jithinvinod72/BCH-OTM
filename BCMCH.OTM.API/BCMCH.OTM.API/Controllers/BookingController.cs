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
        [Route("get-bookings")]
        public async Task<IActionResult> GetBookings(int operationTheatreId=1, string? fromDate="",string? toDate="")
        {
            try
            {
                // bla
                var result = await _bookingService.GetBookingList(operationTheatreId ,  fromDate, toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
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
                return Ok(new ResponseVM<IEnumerable<PostBookingModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpPut]
        [Route("update-booking")]
        public async Task<IActionResult> UpdateBooking(UpdateBookingModel _booking)
        {
            try
            {
                var result = await _bookingService.UpdateBooking(_booking);
                return Ok(new ResponseVM<IEnumerable<UpdateBookingModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
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
