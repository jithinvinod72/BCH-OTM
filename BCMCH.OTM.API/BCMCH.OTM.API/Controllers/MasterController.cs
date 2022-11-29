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
        [Route("get-special-equipments-details")]
        public async Task<IActionResult> SpecialEquipmentsDetails()
        {

            try
            {
                var result = await _masterService.SpecialEquipmentsDetails();
                return Ok(new ResponseVM<IEnumerable<SpecialEquipments>>(true, ResponseMessages.DATA_ACCESS_SUCCESS,result));
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

        #endregion
    }
}