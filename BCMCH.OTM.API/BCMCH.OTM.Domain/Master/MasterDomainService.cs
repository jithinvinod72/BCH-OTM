﻿using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Domain.Contract.Master;
using System.Globalization;
using OfficeOpenXml;

namespace BCMCH.OTM.Domain.Master
{
    public class MasterDomainService : IMasterDomainService
    {
        #region PRIVATE
        private readonly IMasterDataAccess _masterDataAccess;
        #endregion

        #region CONSTRUCTOR
        public MasterDomainService(IMasterDataAccess masterDataAccess)
        {
            _masterDataAccess = masterDataAccess;
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
            
        }
        #endregion


        #region PUBLIC
        public async Task<IEnumerable<Equipments>> GetEquipments()
        {
            var result = await _masterDataAccess.GetEquipments();
            return result;
        }
        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            var result = await _masterDataAccess.GetDepartments();
            return result;
        }
        
        public async Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList()
        {
            var result = await _masterDataAccess.GetAnaesthesiaList();
            return result;
        }
        
        public async Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray,  int pageNumber, int rowsOfPage)
        {
            searchOption.Replace(" ", "%");
            searchOption = "%"+searchOption+"%";
            // replace space with % for sp and 
            // adds % as first and last charecter
            // GetEmployees(string searchOption , string departmentArray, int pageNumber, int rowsOfPage )

            var result = await _masterDataAccess.GetEmployees(searchOption ,departmentArray, pageNumber,rowsOfPage);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode)
        {
            var result = await _masterDataAccess.GetEmployeeDetails(employeeCode);
            return result;
        }
        public async Task<IEnumerable<AvailableRoles>> GetOTRoles()
        {
            var result = await _masterDataAccess.GetOTRoles();
            return result;
        }
        public async Task<UserRole> GetOTUserRole(int employeeId)
        {
            var result = await _masterDataAccess.GetOTUserRole(employeeId);
            var data = result.ElementAt(0);
            var resources =await _masterDataAccess.GetOTRolePermissions(data.UserRoleId);

            UserRole userRole = new UserRole();
            userRole.UserDetails = result;
            userRole.UserResources = resources;
            return userRole;
        }

        public async Task<IEnumerable<int>> PostNewOTUser(UserRoleDetails userRoleDetails)
        {
            var result = await _masterDataAccess.PostNewOTUser(userRoleDetails.EmployeeId,userRoleDetails.UserRoleId);
            return result;
        }
        public async Task<IEnumerable<int>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights)
        {
            var result = await _masterDataAccess.CreateAdminRolesAndRigthts(otAdminAndRights);
            return result;
        }
        public async Task<IEnumerable<Resources>> GetOTResources()
        {
            var result = await _masterDataAccess.GetOTResources();
            return result;
        }

        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatres()
        {
            var result = await _masterDataAccess.GetOperationTheatres();
            return result;
        }

        public async  Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="")
        {
            // Console.WriteLine(_searchKeyword);
            // _searchKeyword = _searchKeyword.Replace(" ", "%");
            // _searchKeyword = "%"+_searchKeyword+"%";
            _searchKeyword="%%";
            var result = await _masterDataAccess.GetSurgeryList(_pageNumber, _rowsPerPage, _searchKeyword);
            return result;
        }
        public async Task<AllMasters> GetMasters()
        {
            AllMasters allMasters = new AllMasters();
            
            allMasters.EquipmentList = await _masterDataAccess.GetEquipments();
            allMasters.AnaesthetistList = await _masterDataAccess.GetEmployees("%%", "[2]", 1, 100 ); // 2 is the department of anaesthetists
            allMasters.OperationTheatreList = await _masterDataAccess.GetOperationTheatres();
            allMasters.AnaesthesiaList = await _masterDataAccess.GetAnaesthesiaList();
            allMasters.DepartmentsList = await _masterDataAccess.GetDepartments();
            allMasters.DateTimeToday = await GetDateToday();

            return allMasters;
        }




        // ----------------------------------------
        // START- Consits functions for allocation 
        #region ALLOCATION_REGION
        // START- private functions to handle allocation
        private DateTime StringToDateTimeConverter( string _date )
        {
            // this function converts input _date string to the date format for dot net
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime parsedDate = DateTime.ParseExact(_date, "yyyy-MM-dd",provider);
            return parsedDate;   
        }
        private int DateDifference(DateTime start, DateTime end)
        {
            // this function returns the number of days between two given dates .
            // For eg : if we give 1/1/2022 and 3/1/2022 it will give the difference as 2 
            return (end.Date-start.Date).Days;
        }
        private string DayFinder(DateTime givenDate)
        {
            // returns day of the week for a given date
            int day = (int)givenDate.DayOfWeek;
            return  day.ToString();
        }
        private List<DateTime> FilterDatesOfDay(DateTime start, DateTime end, string givenDayNumbers)
        {
            // given day numbers will be a comma seperated string eg : 1,2,4
            // we need to it that to an array by using split command
            string[] givenDayNumbersSplitted = givenDayNumbers.Split(',');
            // this contains the array which includes the days to add


            List<DateTime> FilteredDates = new List<DateTime>();
            // used to store the filtered dates.
            int difference = DateDifference(start, end);
            // First we find the number of days between a given start date and end date
            // For eg : if we give 1/1/2022 and 3/1/2022 it will give the difference as 2 

            // after finding the difference we add it to the start date one by one.
            // eg : if the difference is 5 and our start date is 1/1/2022
            // we loop from 0 to 5 
            // and during each iteration we add a day with the start date that is : 1/1/2022
            // so for the first iteration 
            // i=0 and the dateRecurring will be 1/1/2022
            // i=1 and the dateRecurring will be 2/1/2022
            // i=2 and the dateRecurring will be 3/1/2022 
            // ..etc..
            // i=5 and the dateRecurring will be 5/1/2022 
            for (int i = 0; i <= difference; i++)
            {
                DateTime dateRecurring = start.AddDays(i);
                // the above line increments the given date with 1 during each iterations
                string currentDayNumber = DayFinder(dateRecurring);
                // the day finder is a function that return the number of the week day of the given date
                // ie: 0 for Sunday 
                // 1 for Monday .... 6 for Saturday 
                if (givenDayNumbersSplitted.Contains(currentDayNumber))
                {
                    // if the current day number is the givenDayNumber that we want to post
                    // we add it to the FilteredDates function
                    FilteredDates.Add(dateRecurring);
                }
            }
            // atlast we return the filtered adata
            return FilteredDates;
        }
        private DateTime AddDateAndTime(DateTime _date, string _time)
        {
            string[] TimeSplitted = _time.Split(':');
            TimeSpan ts = new TimeSpan(Int32.Parse(TimeSplitted[0]) , Int32.Parse(TimeSplitted[1]), Int32.Parse(TimeSplitted[2]) );
            _date = _date.Date + ts;
            return _date;
        }
        // END- private functions to handle allocation


        // START- public functions for allocation to access from controller
        public async Task<IEnumerable<GetAllocationModel>> GetAllocations(string startDate, string endDate)
        {
            var result = await _masterDataAccess.GetAllocations(startDate,endDate);
            return result;
        }
        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            // used to post allocation with only a startdate,enddate,otid and department id
            var result = await _masterDataAccess.PostAllocation(_allocation);
            return result;
        }
        public async Task<IEnumerable<int>> DeleteAllocations(string allocationIds)
        {
            // allocationIds="["+allocationIds+"]";
            var result = await _masterDataAccess.DeleteAllocations(allocationIds);
            return result;
        }
        
        public async Task<IEnumerable<int>> PostAllocationInARange(AllocateInRange _allocation)
        {
            // used to bulk posting allocation
            // first we convert the incoming startdate and enddate to date in js 
            DateTime start = StringToDateTimeConverter(_allocation.StartDate);
            DateTime end = StringToDateTimeConverter(_allocation.EndDate);

            // SUMMARY : 
            // in this function we give required parameters like start date and end date
            // and the week day number eg: for Sunday the day number will be 0, for Monday the day number will be 1,etc..
            // usingthese we filter out the dates of the days given in between the range that is given.
            List<DateTime> filteredDatesWithDay = FilterDatesOfDay(start, end, _allocation.day);
            // The above function will filter the dates of the day number that we have given within a given start and end dates.
            if (filteredDatesWithDay.Count() < 1) {
                return new List<int> { 1 };
            }

            // we loop through the filteredDatesWithDay and allocate the start and end time with ot and department ids using the PostAllocation function
            foreach (DateTime dateRecurring in filteredDatesWithDay)
            {
                DateTime starDateTime   =  AddDateAndTime(dateRecurring,_allocation.StartTime);
                // the above line adds startDate + startTime time to find the allocation startdatetime
                DateTime endDateTime    =  AddDateAndTime(dateRecurring,_allocation.EndTime);
                // the above line adds endDate + endTime time to find the allocation startdatetime
                string date_time_start  = starDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                // converts to string format which we use to write to database
                string date_time_end    = endDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                // converts to string format which we use to write to database
                
                // year-month-date


                Console.WriteLine();
                Console.Write("datetime start : ");
                Console.Write(date_time_start);
                Console.WriteLine();
                Console.Write("datetime end : ");
                Console.Write(date_time_end);
                Console.WriteLine();

                
                Allocation _postAllocation_format = new Allocation();
                
                _postAllocation_format.OperationTheatreId = _allocation.OperationTheatreId;
                _postAllocation_format.AssignedDepartmentId = _allocation.AssignedDepartmentId;
                _postAllocation_format.StartDate = date_time_start;
                _postAllocation_format.EndDate = date_time_end;
                _postAllocation_format.ModifiedBy = _allocation.ModifiedBy;

                var isAllocationAlreadyExists = await _masterDataAccess.CheckAllocationByOperationThearter(_postAllocation_format.StartDate, _postAllocation_format.EndDate, (int)_postAllocation_format.OperationTheatreId);
                var legnth = isAllocationAlreadyExists.Count();
                if (legnth >0)
                {
                    return new List<int> { 2};
                }
                await PostAllocation(_postAllocation_format);


                // public int?      OperationTheatreId {get; set; }
                // public int?      AssignedDepartmentId {get; set; }
                // public string    StartDate {get; set; }
                // public string    EndDate {get; set; }
                // public int?      ModifiedBy {get; set; }
            }
            // return 0;
            return new List<int> { 0 };
        }
        // END- public functions for allocation to access from controller
        #endregion
        // END- Consits functions for allocation 
        // ----------------------------------------
        
        public async Task<DateTime>GetDateToday()
        {
            var result = await _masterDataAccess.GetDateToday();
            return result.ElementAt(0);
        }
        
        #endregion
        

        #region QUESTION_SECTION
        // insert question section START
        public async Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question)
        {
            var result = await _masterDataAccess.PostQuestion(question);
            return result;
        }
        public async Task<IEnumerable<string>> PostQuestionType(string name,string label)
        {
            var result = await _masterDataAccess.PostQuestionType(name,label);
            return result;
        }
        public async Task<IEnumerable<string>> PostOtStages(string name,string label)
        {
            var result = await _masterDataAccess.PostOtStages(name,label);
            return result;
        }
        // insert question section END

        public async Task<IEnumerable<GetQuestions>> GetFormQuestions(int otStageId, string accessibleTo)
        {
            var result = await _masterDataAccess.GetFormQuestions();
            var filteredResult = result.Where( o=> otStageId==o.otStageId);
            filteredResult = result.Where( o=> accessibleTo==o.accessibleTo);
            // result.Where()
            return filteredResult;
        }
        #endregion

        #region QUESTION_MASTERS_FETCHING
        public async Task<FormMasters> GetFormMasters()
        {
            FormMasters masters= new FormMasters();
            masters.sections = await _masterDataAccess.GetFormSections();
            masters.types = await _masterDataAccess.GetFormQuestionType();
            return masters;
        }
        #endregion

        #region FORM_ANSWER_HANDLE

        public async Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer)
        {
            var result = await _masterDataAccess.PostFormAnswer(answer);
            return result;
        }
        public async Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId)
        {
            var result = await _masterDataAccess.GetFormAnswer(eventId);
            return result;
        }
        #endregion

        
        
    }
}
