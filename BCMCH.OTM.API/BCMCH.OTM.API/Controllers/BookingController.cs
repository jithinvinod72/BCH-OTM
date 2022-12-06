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
        public async Task<IActionResult> GetBookings(int _operationTheatreId=1, string? _fromDate="",string? _toDate="")
        {
            try
            {
                // bla
                var result = await _bookingService.GetBookingList(_operationTheatreId ,  _fromDate, _toDate);
                return Ok(new ResponseVM<IEnumerable<BookingResponse>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("post-booking")]
        public async Task<IActionResult> PostBooking(PostBookingModel _booking)
        {
            try
            {
                var result = await _bookingService.PostBooking(_booking);
                return Ok(new ResponseVM<IEnumerable<PostBookingModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        // [HttpGet]
        // [Route("is-perationtheatre-allocated")]
        // public async Task<IActionResult> IsOperationTheatreAllocated()
        // {
        //     try
        //     {
        //         var test = await _bookingService.IsOperationTheatreAllocated();
        //         return Ok(new ResponseVM<int>(true, ResponseMessages.DATA_ACCESS_SUCCESS, test ));

        //     }
        //     catch (Exception ex)
        //     {
        //         return Ok(new ResponseVM<bool>(false, ex.Message));
        //     }
        // }
        #endregion

        

    }
}
