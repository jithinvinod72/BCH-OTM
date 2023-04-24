using System.Globalization;
using System.Text.Json;
using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using BCMCH.OTM.Infrastucture.Generic;
using OfficeOpenXml;

namespace BCMCH.OTM.Domain.Booking
{
    
    public class BookingDomainService : IBookingDomainService
    {
        private readonly IBookingDataAccess _bookingDataAccess;
        #region CONSTRUCTOR
        public BookingDomainService(IBookingDataAccess bookingDataAccess)
        {
            _bookingDataAccess = bookingDataAccess;
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
        }
        #endregion

        private DateTime StringToDateTimeConverter( string dateTime )
        {
            var parsedDate = DateTime.ParseExact(dateTime, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            return parsedDate;
        }

        public async Task<IEnumerable<Bookings>> GetBookingList(string fromDate, string toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            return result;
        }

        public async Task<IEnumerable<Bookings>> GetBookingListWithDepartment(string departmentIds, string? fromDate,string? toDate)
        {
            var departments = JsonSerializer.Deserialize<List<int?>>(departmentIds);
            // the input departmentIds will be a json array convert it into c# array 
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            // then ge the bookings with start and end date
            var filteredWithDepartment = result.Where(booking=> departments.Contains(booking.BookedByDepartment) );
            // then filter the bookings with the department ids array 
            return filteredWithDepartment;
        }
        public async Task<IEnumerable<Bookings>> GetBookingListWithOtId(string otIds, string? fromDate,string? toDate)
        {
            var otsSelected = JsonSerializer.Deserialize<List<int>>(otIds);

            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            var filteredWithOtId = result.Where(booking=> otsSelected.Contains(booking.OperationTheatreId) );
            return filteredWithOtId;
        }
        
        public async Task<IEnumerable<Bookings>> GetBookingsSorted(bool PaginationEnabled=false, int pageNumber=0,string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            // remove waitinglist 
            result = result.Where(booking=> booking.StatusCode!=4 );
            result = result.Where(booking=> booking.StatusCode!=3 );
            
            
            var queryResultPage = result;

            // if pagination is enabled we will add the functions for that
            if(PaginationEnabled==true){
                int numberOfObjectsPerPage = 20;
                queryResultPage = result
                    .Skip(numberOfObjectsPerPage * pageNumber)
                    .Take(numberOfObjectsPerPage);
            }
            // if pagination is enabled we will add the functions for that

            switch (sortValue)
            {
                case "PATIENT_NAME":
                    if(sortType=="DESCENDING"){
                        return queryResultPage.OrderByDescending(s => s.PatientFirstName);
                    }
                    return queryResultPage.OrderBy(s => s.PatientFirstName);

                case "PATIENT_AGE":
                    if(sortType=="DESCENDING"){
                        return queryResultPage.OrderByDescending(s => s.PatientDateOfBirth);
                    }
                    return queryResultPage.OrderBy(s => s.PatientDateOfBirth);
                    
                case "SURGERY_NAME":
                    if(sortType=="DESCENDING"){
                        return queryResultPage.OrderByDescending(s => s.SurgeryName);
                    }
                    return queryResultPage.OrderBy(s => s.SurgeryName);

                case "DEPARTMENT":
                    if(sortType=="DESCENDING"){
                        return queryResultPage.OrderByDescending(s => s.DepartmentName);
                    }
                    return queryResultPage.OrderBy(s => s.DepartmentName);

                case "OPERATION_THEATRE_NAME":
                    if(sortType=="DESCENDING"){
                        return queryResultPage.OrderByDescending(s => s.TheatreName);
                    }
                    return queryResultPage.OrderBy(s => s.TheatreName);
                case "OPERATION_TIME":
                    if(sortType=="ASCENDING"){
                        var StartDateOrderAsc = queryResultPage.OrderBy(s => s.StartDate);
                        return StartDateOrderAsc;
                    }
                    var StartDateOrderDes = queryResultPage.OrderByDescending(s => s.StartDate);
                    return StartDateOrderDes;
                default:
                    return queryResultPage;
            }
            
        }
        // EXCEL Handlers START
        public async Task<Stream> ExportEvents( string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            Console.WriteLine("fromdate : ", fromDate);
            Console.WriteLine("todate : ", toDate);
            var result = await GetBookingsSorted(false, 0,sortValue , sortType , fromDate, toDate);
            // here there will not be any paginations applied 
            
            var stream = new MemoryStream();
            var package = new OfficeOpenXml.ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Attempts");
            worksheet = package.Workbook.Worksheets.Add("Assessment Attempts");
            int rowCounter =1;
            // UHID 	Name 	Age 	Gender 	Surgery 	Department 	Operation Treater 	Date Time
            worksheet.Cells[rowCounter, 1].Value = "event id ";
            worksheet.Cells[rowCounter, 2].Value = "UHID";
            worksheet.Cells[rowCounter, 3].Value = "Name";
            worksheet.Cells[rowCounter, 4].Value = "Age";
            worksheet.Cells[rowCounter, 5].Value = "Gender";
            worksheet.Cells[rowCounter, 6].Value = "Surgery";
            worksheet.Cells[rowCounter, 7].Value = "Department";
            worksheet.Cells[rowCounter, 8].Value = "Operation Theatre";
            worksheet.Cells[rowCounter, 9].Value = "Start Time";
            
            foreach (Bookings item in result)
            {
                if (item.PatientRegistrationNo != null)
                {
                    rowCounter++;

                    worksheet.Cells[rowCounter, 1].Value = item.event_id;

                    worksheet.Cells[rowCounter, 2].Value = item.PatientRegistrationNo;
                    worksheet.Cells[rowCounter, 3].Value = item.PatientFirstName + " " + item.PatientMiddleName + " " + item.PatientLastName;
                    worksheet.Cells[rowCounter, 4].Value = FindAgeFromDOB(item.PatientDateOfBirth);
                    worksheet.Cells[rowCounter, 5].Value = item.PatientGender == 1 ? "Female" : "Male";
                    worksheet.Cells[rowCounter, 6].Value = item.SurgeryPrintName;
                    worksheet.Cells[rowCounter, 7].Value = item.DepartmentName;
                    worksheet.Cells[rowCounter, 8].Value = item.TheatreName;
                    worksheet.Cells[rowCounter, 9].Value = item.StartDate.ToString();
                }
            }
            
            for (int i = 1; i <= 9; i++) {
                worksheet.Column(i).AutoFit(); 
            }

            package.Workbook.Properties.Title = "summary";
            package.Save();
            stream.Position=0;
            return stream;
        }
        public async Task<EventFields> GetEventEquipmentsAndEmployees(int bookingId)
        {
            var equipments = await _bookingDataAccess.GetEventEquipments(bookingId);
            var employees  = await _bookingDataAccess.GetEventEmployees(bookingId);
            var departmentIds = employees.Select(o => o.DepartmentID).Distinct();

            var allDepartments = await _bookingDataAccess.GetDepartments();
            var filteredDepartments = allDepartments.Where(o=>  departmentIds.Contains(o.Id));
            
            var fields = new EventFields();
            fields.Equipments = equipments;
            fields.Surgeons = employees;
            fields.Departments = filteredDepartments;
            
            return fields;
        }

        public async Task<IEnumerable<Bookings>> DeleteBooking(string IdArray="")
        {
            var result = await _bookingDataAccess.DeleteBooking(IdArray);
            return result;
        }

        public async Task<Envelope<IEnumerable<int>>> AddBooking(PostBookingModel booking)
        {

            // convertTimeTwelveToTwentyFour(booking.EndDate);
            #region VALIDATION
            // START - VALIDATION SECTION   
            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if (OTAllocationStatus < 1)
            {
                return new Envelope<IEnumerable<int>>(false, $"Selected Operation theatre is not allocated for the given time {StringToDateTimeConverter(booking.StartDate)} and {StringToDateTimeConverter(booking.EndDate)}");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<int>>(false, $"The selected operation theatre is blocked in the given time {StringToDateTimeConverter(booking.StartDate)} and {StringToDateTimeConverter(booking.EndDate)}");
            }

            var OTBookingStatus = await _bookingDataAccess.IsOperationTheatreBooked(0, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if (OTBookingStatus > 0)
            {
                
                return new Envelope<IEnumerable<int>>(false, $"Operation Theatre is already booked for the slot {StringToDateTimeConverter(booking.StartDate)} to {StringToDateTimeConverter(booking.EndDate)}");
            }
            // END - VALIDATION SECTION
            #endregion        

            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            booking.SurgeriesIdArray="["+booking.SurgeriesIdArray+"]";
            
            var result = await _bookingDataAccess.AddBooking(booking);
            return new Envelope<IEnumerable<int>>(true, "booking created", result); ;
        }

        public async Task<Envelope<IEnumerable<int>>> AddWaitingList(PostBookingModel booking)
        {
            // here there are no validations required. 
            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            var result = await _bookingDataAccess.AddBooking(booking);
            return new Envelope<IEnumerable<int>>(true, "booking created", result); ;
        }
        public async Task<IEnumerable<Surgeries>> GetEventSurgeries(int bookingId){
            var result = await _bookingDataAccess.GetEventSurgeries(bookingId);
            return result;
        }

        public async Task<Envelope<IEnumerable<UpdateBookingModel>>> UpdateBooking(UpdateBookingModel booking)
        {

            #region VALIDATION  
            if(booking.StatusId==5){
                booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
                booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
                var emerGencyResult = await _bookingDataAccess.UpdateBooking(booking);
                return new Envelope<IEnumerable<UpdateBookingModel>>(true,"data-update-success", emerGencyResult);
            }

            var OTAllocationStatus = await _bookingDataAccess.IsOperationTheatreAllocated(booking.OperationTheatreId, booking.DepartmentId, booking.StartDate, booking.EndDate);
            if(OTAllocationStatus < 1)
            {
               return new Envelope<IEnumerable<UpdateBookingModel>>(false,$"Selected Operation theatre is not allocated in the given time {StringToDateTimeConverter(booking.StartDate)} and {StringToDateTimeConverter(booking.EndDate)}");
            }

            var OTBlockStatus = await _bookingDataAccess.IsOperationTheatreBloked(booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBlockStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"The selected operation theatre is blocked in the given time {StringToDateTimeConverter(booking.StartDate)} and {StringToDateTimeConverter(booking.EndDate)}");
            }

            var OTBookingStatus    = await _bookingDataAccess.IsOperationTheatreBooked(booking.Id, booking.OperationTheatreId, booking.StartDate, booking.EndDate);
            if(OTBookingStatus > 0)
            {
                return new Envelope<IEnumerable<UpdateBookingModel>>(false, $"Operation Theatre is already booked in the given time {StringToDateTimeConverter(booking.StartDate)} and {StringToDateTimeConverter(booking.EndDate)}");
            }
            #endregion


            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            var result = await _bookingDataAccess.UpdateBooking(booking);
            return new Envelope<IEnumerable<UpdateBookingModel>>(true,"data-update-success", result);
        }

      

        public async Task<Envelope<IEnumerable<Blocking>>> AddBlocking(Blocking blocking)
        {
            if( StringToDateTimeConverter(blocking.StartDate)> StringToDateTimeConverter(blocking.EndDate)){
                return new Envelope<IEnumerable<Blocking>>(false, $"given star time is less than end time. Please choose valid start and end time.");
            }
            var result = await _bookingDataAccess.AddBlocking(blocking);
            return new Envelope<IEnumerable<Blocking>>(true,"data-update-success", result);
        }
        public async Task<Envelope<IEnumerable<Blocking>>> EditBlocking(Blocking blocking)
        {
            if( StringToDateTimeConverter(blocking.StartDate)> StringToDateTimeConverter(blocking.EndDate)){
                return new Envelope<IEnumerable<Blocking>>(false, $"given star time is less than end time. Please choose valid start and end time.");
            }

            var result = await _bookingDataAccess.EditBlocking(blocking);
            return new Envelope<IEnumerable<Blocking>>(true,"data-update-success", result);
            // return result;
        }


        public async Task<BookingsAndAllocations> SelectBookingsAndAllocations(int departmentId,int operationTheatreId , string? fromDate,string? toDate)
        {
            // selects bookings and allocations in accordance with given 
            // departmentId
            // operationTheatreId
            // fromDate
            // toDate
            var bookings = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            bookings = bookings.Where(
                                        booking => (
                                                    (booking.OperationTheatreId == operationTheatreId) 
                                                    && 
                                                    ((departmentId == booking.BookedByDepartment) || (booking.StatusName=="BLOCKED") )
                                                 )
                                     );
            // filters the bookings with given otid and department id 

            var allocations = await _bookingDataAccess.GetAllocation(fromDate, toDate);
            // fetches allocation details of the given departments
            var filteredAllocations = allocations.Where( 
                                                            o=> (
                                                                    (operationTheatreId==o.OperationTheatreId)
                                                                    &&
                                                                    (departmentId==o.AssignedDepartmentId)
                                                                ) 
                                                        );
            // filter allocations for given operation theatre id and AssignedDepartmentId
            // var surgeryMapping = new IEnumerable<Surgeries>();
            // List<Surgeries> list = new List<Surgeries>();
            foreach (var item in bookings)
            {
                var surgeriesMapping = await _bookingDataAccess.GetEventSurgeries(item.event_id);
                // list.Add((Surgeries)surgeriesMapping);
                item.SurgeriesMapping = (List<Surgeries>)surgeriesMapping;
            }
            
            // var surgeriesMapping = await _bookingDataAccess.GetEventSurgeries(bookingId);

            // return result;

            var result = new BookingsAndAllocations();
            result.Bookings = bookings;
            // result.Bookings.
            result.Allocations = filteredAllocations;
            return result;
        }
        public async Task<IEnumerable<int?>> SelectAllocatedTheatres(int departmentId, string? fromDate,string? toDate)
        {
            // sellects the unique operation theatre id those are allocated for the given departments
            // in accordance with given startDate and endDate
            var allocations = await _bookingDataAccess.GetAllocation(fromDate, toDate);
            var filteredAllocations = allocations.Where( o=> departmentId==o.AssignedDepartmentId);
            var allocatedOperationtheatres = filteredAllocations.Select(o => o.OperationTheatreId).Distinct();
            
            // above line selects unique OperationTheatreId from the allocations 
            // used to show drop down in frontend             
            var result = allocatedOperationtheatres;
            return result;
        }

        public async Task<IEnumerable<Patient>> GetPatientData(string registrationNo)
        {
            var result = await _bookingDataAccess.GetPatientData(registrationNo);
            return result;
        }

        private string FindAgeFromDOB(string DOB) {
            if(DOB == null) {
                return "NA";
            }

            var year = DateTime.ParseExact(DOB, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture).Date.Year;
            var currentYear = DateTime.Now.Year;
            return (currentYear - year).ToString();
        }

    }
}
