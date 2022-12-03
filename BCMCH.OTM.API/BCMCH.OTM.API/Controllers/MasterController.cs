using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.ViewModels.Generic;
using BCMCH.OTM.API.ViewModels.ResponseMessage;
using BCMCH.OTM.Domain.Contract.Master;
using Microsoft.AspNetCore.Mvc;

namespace BCMCH.OTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        #region PRIVATE
        private readonly IMasterDomainService _masterService;
        #endregion

        #region CONSTRUCTOR
        public MasterController(IMasterDomainService masterService)
        {
            _masterService = masterService;
        }
        #endregion

        #region PUBLIC
        [HttpGet]
        [Route("get-equipments-details")]
        public async Task<IActionResult> EquipmentsDetails()
        {

            try
            {
                var result = await _masterService.EquipmentsDetails();
                return Ok(new ResponseVM<IEnumerable<Equipments>>(true, ResponseMessages.DATA_ACCESS_SUCCESS,result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        
        [HttpGet]
        [Route("get-departments-details")]
        public async Task<IActionResult> Departments()
        {
            try
            {
                var result = await _masterService.DepartmentDetails();
                return Ok(new ResponseVM<IEnumerable<Departments>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-anasthesia-list")]
        public async Task<IActionResult> GetAnaesthesiaList()
        {
            try
            {
                var result = await _masterService.GetAnaesthesiaList();                
                return Ok(new ResponseVM<IEnumerable<Anaesthesia>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-employee-list")]
        public async Task<IActionResult> GetEmployeeList(string? SearchKeyword=""  , string departments="[]" )
        {
            try
            {
                var result = await _masterService.GetEmployeeList(SearchKeyword,departments);
                return Ok(new ResponseVM<IEnumerable<Employee>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-operationtheatre-allocation")]
        public async Task<IActionResult> GetOperationTheatreAllocations(int _departmentId, string? _fromDate)
        {
            try
            {
                var result = await _masterService.GetOperationTheatreAllocations(_departmentId, _fromDate);
                return Ok(new ResponseVM<IEnumerable<OperationTheatreAllocation>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-operationtheatre-list")]
        public async Task<IActionResult> GetOperationTheatreAllocations()
        {
            try
            {
                var result = await _masterService.GetOperationTheatreList();
                return Ok(new ResponseVM<IEnumerable<OperationTheatre>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-surgery-list")]
        public async Task<IActionResult> GetSurgeryList(int _pageNumber=1, int _rowsPerPage=100, string? _searchKeyword="")
        {
            try
            {
                var result = await _masterService.GetSurgeryList(_pageNumber, _rowsPerPage, _searchKeyword);
                return Ok(new ResponseVM<IEnumerable<Surgery>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-bookings-list")]
        public async Task<IActionResult> GetBookingsList(int _operationTheatreId=1, string? _fromDate="",string? _toDate="")
        {
            try
            {
                // bla
                var result = await _masterService.GetBookingList(_operationTheatreId ,  _fromDate, _toDate);
                return Ok(new ResponseVM<IEnumerable<Bookings>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        #endregion
    }
}

