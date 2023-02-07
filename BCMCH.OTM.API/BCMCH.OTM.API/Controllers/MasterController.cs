﻿using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.API.ViewModels.Generic;
using BCMCH.OTM.API.ViewModels.ResponseMessage;
using BCMCH.OTM.Domain.Contract.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using BCMCH.OTM.External;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BCMCH.OTM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController : ControllerBase
    {
        #region PRIVATE
        private readonly IMasterDomainService _masterService;
        private readonly IOTMDataClient _iOTMDataClient;
        private readonly IConfiguration _configuration;
        #endregion

        #region CONSTRUCTOR
        public MasterController(IMasterDomainService masterService, IOTMDataClient iOTMDataClient, IConfiguration configuration)
        {
            _masterService = masterService;
            _iOTMDataClient = iOTMDataClient;
            _configuration = configuration;
        }
        #endregion

        #region PUBLIC
        [HttpGet]
        [Route("get-equipments")]
        public async Task<IActionResult> GetEquipments()
        {

            try
            {
                var result = await _masterService.GetEquipments();
                return Ok(new ResponseVM<IEnumerable<Equipments>>(true, ResponseMessages.DATA_ACCESS_SUCCESS,result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        

        [HttpGet]
        [Route("get-user-auth")]
        public async Task<IActionResult> GetUserAuth()
        {

            Console.WriteLine("here");
            try
            {

                if (!Request.Headers.ContainsKey("Authorization"))
                    Ok(new ResponseVM<bool>(false, "Missing Authorization Header"));

                
                string url= _configuration.GetSection("baseURL").Value + @"user/validate";


                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                Console.WriteLine(authHeader);
                var token = authHeader.Parameter;

                BodyExternalAPI key = new BodyExternalAPI();
                key.key = token;

                var result =await _iOTMDataClient.PostAsync<UserDetails>(url, key);

                if (result == null) { 
                    return Ok(new ResponseVM<bool>(false, ResponseMessages.DATA_NOT_FOUND));
                }

                return Ok(new ResponseVM<UserDetails>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpGet]
        [Route("get-user-details")]
        public async Task<IActionResult> GetUserDetails(int employeeCode)
        {
            try
            {
                var result = await _masterService.GetEmployeeDetails(employeeCode);
                return Ok(new ResponseVM<IEnumerable<Employee>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-departments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var result = await _masterService.GetDepartments();
                return Ok(new ResponseVM<IEnumerable<Departments>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-anasthesiatypes")]
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
        [Route("get-employees")]
        public async Task<IActionResult> GetEmployees(string? searchKeyword=""  , string departments="[]", int pageNumber=1, int rowsOfPage=100 )
        {
            try
            {
                var result = await _masterService.GetEmployees(searchKeyword,departments, pageNumber, rowsOfPage);
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
        [Route("get-operationtheatres")]
        public async Task<IActionResult> GetOperationTheatres()
        {
            try
            {
                var result = await _masterService.GetOperationTheatres();
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
        [Route("get-masters")]
        public async Task<IActionResult> GetMasters()
        {
            try
            {
                var result = await _masterService.GetMasters();
                return Ok(new ResponseVM<AllMasters>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpPost]
        [Route("post-Allocation")]
        public async Task<IActionResult> PostAllocation( Allocation _allocation)
        {
            try
            {
                var result = await _masterService.PostAllocation(_allocation);
                return Ok(new ResponseVM<IEnumerable<Allocation>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }


        [HttpGet]
        [Route("get-allocation")]
        public async Task<IActionResult> GetAllocation(int departmentId, string startDate, string endDate)
        {
            try
            {
                var result = await _masterService.GetAllocation(departmentId, startDate, endDate);
                return Ok(new ResponseVM<IEnumerable<GetAllocationModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-today")]
        public async Task<IActionResult> GetToday()
        {
            try
            {
                var result = await _masterService.GetDateToday();
                return Ok(new ResponseVM<DateTime>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        #endregion
    }
}



