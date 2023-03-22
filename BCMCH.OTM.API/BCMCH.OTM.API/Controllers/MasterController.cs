using BCMCH.OTM.API.Shared.Master;
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
        [Route("get-user-role")]
        public async Task<IActionResult> GetOTUserRole(int employeeId)
        {
            try
            {
                var result = await _masterService.GetOTUserRole(employeeId);
                return Ok(new ResponseVM<UserRole>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpGet]
        [Route("get-available-roles")]
        public async Task<IActionResult> GetOTRoles()
        {
            try
            {
                var result = await _masterService.GetOTRoles();
                return Ok(new ResponseVM<IEnumerable<AvailableRoles>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
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

        [HttpPost]
        [Route("post-new-ot-user")]
        public async Task<IActionResult> PostNewOTUser(UserRoleDetails userRole)
        {
            try
            {
                var result = await _masterService.PostNewOTUser(userRole);                
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("post-new-admin-roles-and-rights")]
        public async Task<IActionResult> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights)
        {
            try
            {
                var result = await _masterService.CreateAdminRolesAndRigthts(otAdminAndRights);                
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // Task<IEnumerable<int>> CreateNewOTAdminAndRights(PostOtAdminAndRights otAdminAndRights)

        [HttpGet]
        [Route("get-resources")]
        public async Task<IActionResult> GetOTResources()
        {
            try
            {
                var result = await _masterService.GetOTResources();
                return Ok(new ResponseVM<IEnumerable<Resources>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        // Task<IEnumerable<Resources>> GetOTResources();

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

        [HttpPost]
        [Route("post-Allocation-in-a-range")]
        public async Task<IActionResult> PostAllocationInARange( AllocateInRange _allocation)
        {
            try
            {
                // Task<int> PostAllocationInARange(AllocateInRange _allocation);
                var result = await _masterService.PostAllocationInARange(_allocation);
                if (result.Contains(0))
                {
                    return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result));
                }
                else if (result.Contains(1))
                {
                    return Ok(new ResponseVM<IEnumerable<int>>(false, ResponseMessages.IS_ALLOCATION_DAY_VALIDATION, result));
                }
                else if (result.Contains(1))
                {
                    return Ok(new ResponseVM<IEnumerable<int>>(false, ResponseMessages.IS_ALLOCATION_EXISTS, result));
                }
                else 
                {
                    return Ok(new ResponseVM<IEnumerable<int>>(false, ResponseMessages.SOMETHING_WENT_WRNG, result));
                }

            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpGet]
        [Route("get-allocation")]
        public async Task<IActionResult> GetAllocations(string startDate, string endDate)
        {
            try
            {
                var result = await _masterService.GetAllocations( startDate, endDate);
                return Ok(new ResponseVM<IEnumerable<GetAllocationModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpDelete]
        [Route("delete-allocations")]
        public async Task<IActionResult> DeleteAllocations(string? allocationIds)
        {
            try
            {
                // Task<int> PostAllocationInARange(AllocateInRange _allocation);
                var result = await _masterService.DeleteAllocations(allocationIds);
                return Ok(new ResponseVM<IEnumerable<int>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
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


        #region QUESTION_SECTION
        // Question section post START
        [HttpPost]
        [Route("post-questions")]
        public async Task<IActionResult> PostQuestion(PostQuestionsModel question)
        {
            // {
            //   "formsectionId": 1,
            //   "formQuestionType": 1,
            //   "order": 1,
            //   "name": "name",
            //   "question": "question",
            //   "parentId": 0,
            //   "rolesToShow": "ADMIN",
            //   "questionTypeId": 0,
            //   "options": "{\"options\":{[\"yes\",\"no\"]}}"
            // }
            try
            {
                var result = await _masterService.PostQuestion(question);
                return Ok(new ResponseVM<IEnumerable<PostQuestionsModel>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("post-question-type")]
        public async Task<IActionResult> PostQuestionType(string name,string label)
        {
            try
            {
                var result = await _masterService.PostQuestionType(name, label);
                return Ok(new ResponseVM<IEnumerable<string>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("post-form-stages")]
        public async Task<IActionResult>PostOtStages(string name,string label)
        {
            try
            {
                var result = await _masterService. PostOtStages(name,label);
                return Ok(new ResponseVM<IEnumerable<string>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // Question section post END
        // Question section Fetch START
        [HttpGet]
        [Route("get-form-questions")]
        public async Task<IActionResult> GetFormQuestions(int otStageId, string accessibleTo)
        {
            try
            {
                var result = await _masterService.GetFormQuestions(otStageId,accessibleTo);
                return Ok(new ResponseVM<IEnumerable<GetQuestions>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // Question section Fetch END

        // Question section Fetch START
        [HttpGet]
        [Route("get-form-masters")]
        public async Task<IActionResult> GetFormMasters()
        {
            try
            {
                var result = await _masterService.GetFormMasters();
                return Ok(new ResponseVM<FormMasters>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        // Question section Fetch END

        #endregion

        #region FORM_ANSWER_HANDLE
        [HttpPost]
        [Route("post-form-answers")]
        public async Task<IActionResult> PostFormAnswers(PostAnswer answer)
        {
            try
            {
                var result = await _masterService.PostFormAnswer(answer);
                return Ok(new ResponseVM<IEnumerable<PostAnswer>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        [HttpGet]
        [Route("get-form-answers")]
        public async Task<IActionResult> GetFormAnswers(int eventId)
        {
            try
            {
                var result = await _masterService.GetFormAnswer(eventId);
                return Ok(new ResponseVM<IEnumerable<GetAnswer>>(true, ResponseMessages.DATA_ACCESS_SUCCESS, result ));
            }
            catch (Exception ex)
            {
                return Ok(new ResponseVM<bool>(false, ex.Message));
            }
        }
        #endregion
        
        
    }
}



