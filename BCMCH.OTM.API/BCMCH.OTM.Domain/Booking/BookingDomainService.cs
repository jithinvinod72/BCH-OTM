using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using BCMCH.OTM.API.Shared.Booking;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Domain.Contract.Booking;
using BCMCH.OTM.Infrastucture.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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


        private float? CalculateHourDifference(DateTime? startTime, DateTime? endTime)
        {
            if ((!startTime.HasValue || startTime == DateTime.MinValue) || (!endTime.HasValue || endTime == DateTime.MinValue))
            {
                return 0;
            }
            if(endTime < startTime){
                var temp    = startTime;
                startTime   = endTime ;
                endTime     = temp;
            }
            TimeSpan difference = (TimeSpan)(endTime - startTime);
            float hourDifference = (float)(difference.TotalMinutes - 10) / 60;
            int minuteDifference = (int)difference.TotalMinutes;

            return minuteDifference;
        }


        // used to fetch surgery id using booking id and populate the booking ienumerable 
        private async Task<IEnumerable<Bookings>> PopulateBookingsWithSurgeries(IEnumerable<Bookings> bookings)
        {
            // used to fetch details of each operations with operation id from booking id and adds to each operations
            foreach (var item in bookings)
            {
                var surgeriesMapping = await _bookingDataAccess.GetEventSurgeries(item.event_id);
                item.SurgeriesMapping = (List<Surgeries>)surgeriesMapping;
                item.SurgeriesSelectedString  = string.Join(", \n", item.SurgeriesMapping.Select(s => s.Name));
                item.AverageSurgeryTime = CalculateHourDifference(item.OtEntryTime, item.OtExitTime);
            }
            
            return bookings;
        }

    
        private DateTime StringToDateTimeConverter( string dateTime )
        {
            var parsedDate = DateTime.ParseExact(dateTime, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            return parsedDate;
        }

        public async Task<IEnumerable<Bookings>> GetBookingList(string fromDate, string toDate)
        {
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            result = (IEnumerable<Bookings>) await PopulateBookingsWithSurgeries(result);
            return result;
        }

        public async Task<IEnumerable<Bookings>> GetBookingListWithDepartment(string departmentIds, string? fromDate,string? toDate)
        {
            var departments = JsonSerializer.Deserialize<List<int?>>(departmentIds);
            // the input departmentIds will be a json array convert it into c# array 
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            // then ge the bookings with start and end date
            var filteredWithDepartment = result.Where(booking=> departments.Contains(booking.BookedByDepartment) );
            filteredWithDepartment = (IEnumerable<Bookings>) await PopulateBookingsWithSurgeries(filteredWithDepartment);
            // then filter the bookings with the department ids array 
            return filteredWithDepartment;
        }
        public async Task<IEnumerable<Bookings>> GetBookingListWithOtId(string otIds, string? fromDate,string? toDate)
        {
            var otsSelected = JsonSerializer.Deserialize<List<int>>(otIds);

            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            var filteredWithOtId = result.Where(booking=> otsSelected.Contains(booking.OperationTheatreId) );
            filteredWithOtId = (IEnumerable<Bookings>) await PopulateBookingsWithSurgeries(filteredWithOtId);
            return filteredWithOtId;
        }
        
        public async Task<IEnumerable<Bookings>> GetBookingsSorted(bool PaginationEnabled=false, int pageNumber=0,string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            
            var result = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            result = (IEnumerable<Bookings>) await PopulateBookingsWithSurgeries(result);
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
                case "COMPLEX_ENTRY":
                    if(sortType=="ASCENDING"){
                        var StartDateOrderAsc = queryResultPage.OrderBy(s => s.OtComplexEntry);
                        return StartDateOrderAsc;
                    }
                    var complexEntryDesc = queryResultPage.OrderByDescending(s => s.OtComplexEntry);
                    return complexEntryDesc;
                case "OT_ENTRY":
                    if(sortType=="ASCENDING"){
                        var StartDateOrderAsc = queryResultPage.OrderBy(s => s.OtEntryTime);
                        return StartDateOrderAsc;
                    }
                    var otEntryDesc = queryResultPage.OrderByDescending(s => s.OtEntryTime);
                    return otEntryDesc;
                case "POSTOP_ENTRY":
                    if(sortType=="ASCENDING"){
                        var StartDateOrderAsc = queryResultPage.OrderBy(s => s.PostOpEntryTime);
                        return StartDateOrderAsc;
                    }
                    var postOpDesc = queryResultPage.OrderByDescending(s => s.PostOpEntryTime);
                    return postOpDesc;

                case "POSTOP_EXIT":
                    if(sortType=="ASCENDING"){
                        var StartDateOrderAsc = queryResultPage.OrderBy(s => s.PostOpExitTime);
                        return StartDateOrderAsc;
                    }
                    var postOpExit = queryResultPage.OrderByDescending(s => s.PostOpExitTime);
                    return postOpExit;

                default:
                    return queryResultPage;
            }
            
        }
        // EXCEL Handlers START
        public async Task<Stream> ExportEvents( string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            var result = await GetBookingsSorted(false, 0,sortValue , sortType , fromDate, toDate);
            
            // here there will not be any paginations applied 
            var stream = new MemoryStream();
            var package = new OfficeOpenXml.ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Attempts");
            // worksheet.Cells.AutoFitColumns();
            worksheet = package.Workbook.Worksheets.Add("Assessment Attempts");
            int rowCounter =1;
            
            // Heading Array STOP
            string[] values = new string[]
            {
                "event id",
                "UHID",
                "Name",
                "Age",
                "Gender",
                "Surgery",
                "Department",
                "Operation Theatre",
                "Scheduled start Time",
                "Ot Complex Entry Time",
                "Pre Op Entry Time",
                "OT Entry Time",
                "Post Op Entry Time",
                "Post Op Exit Time",
                "Average Surgery Time"
            };
            // Heading Array STOP

            // Write headings to sheet using looop
            for (int i = 0; i < values.Length; i++)
            {
                worksheet.Cells[rowCounter, i + 1].Value = values[i];
                worksheet.Cells[rowCounter, i + 1].Style.Font.Bold = true;
            }
            // Write headings to sheet using looop
            
            // body part 
            foreach (Bookings item in result)
            {
                if (item.PatientRegistrationNo != null)
                {
                    rowCounter++;
                    worksheet.Cells[rowCounter, 1].Value  = item.event_id;
                    worksheet.Cells[rowCounter, 2].Value  = item.PatientRegistrationNo;
                    worksheet.Cells[rowCounter, 3].Value  = item.PatientFirstName + " " + item.PatientMiddleName + " " + item.PatientLastName;
                    worksheet.Cells[rowCounter, 4].Value  = FindAgeFromDOB(item.PatientDateOfBirth);
                    worksheet.Cells[rowCounter, 5].Value  = item.PatientGender == 1 ? "Female" : "Male";
                    worksheet.Cells[rowCounter, 6].Value  = item.SurgeriesSelectedString;
                    worksheet.Cells[rowCounter, 7].Value  = item.DepartmentName;
                    worksheet.Cells[rowCounter, 8].Value  = item.TheatreName;
                    worksheet.Cells[rowCounter, 9].Value  = item.StartDate.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 10].Value = item.OtComplexEntry?.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 11].Value = item.PreOpEntryTime?.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 12].Value = item.OtEntryTime?.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 13].Value = item.PostOpEntryTime?.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 14].Value = item.PostOpExitTime?.ToString()  ??string.Empty;
                    worksheet.Cells[rowCounter, 15].Value = item.AverageSurgeryTime;
                    // worksheet.Cells[rowCounter, 14].Value = item.PostOpExitTime?.ToString()  ??string.Empty;
                }
            }
            
            for (int i = 1; i <= values.Length; i++) {
                worksheet.Column(i).AutoFit(); 
            }

            package.Workbook.Properties.Title = "summary";
            package.Save();
            stream.Position=0;
            return stream;
        }

        public static MemoryStream ConvertIEnumerableToExcelStream<T>(string[] headerArray, IEnumerable<T> collection, string sheetName, params object[] disabledColumns)
        {
            // headerArray -> used to add header row to the excel. 
            // if is [] the header is taken from the object value names . 

            MemoryStream stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                int colCounter=0;
                // Add column headers START
                
                if(headerArray.Length>0)
                {
                    // if header array exists 
                    for (int columnIndex = 1; columnIndex <= properties.Length; columnIndex++)
                    {
                        worksheet.Cells[1, columnIndex].Value = headerArray[columnIndex - 1];
                        worksheet.Cells[1, columnIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                } else {
                    // else  take header  from object names 
                    for (int columnIndex = 1; columnIndex <= properties.Length; columnIndex++)
                    {
                        PropertyInfo property = properties[columnIndex - 1];
                        // filter out unwanted columns from heading START
                        if (disabledColumns.Contains(property.Name) || disabledColumns.Contains(columnIndex))
                        {
                            continue; // Skip disabled columns
                        }
                        // filter out unwanted columns from heading END
                        colCounter++;
                        worksheet.Cells[1, colCounter].Value = properties[columnIndex - 1].Name;
                        worksheet.Cells[1, colCounter].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                }
                
                // Add column headers END

                // Add data rows START

                
                int rowIndex = 2;
                
                foreach (T item in collection)
                {
                    colCounter=1;
                    for (int columnIndex = 1; columnIndex <= properties.Length; columnIndex++)
                    {
                        
                        PropertyInfo property = properties[columnIndex - 1];
                        // filter out unwanted columns from body START
                        if (disabledColumns.Contains(property.Name) || disabledColumns.Contains(columnIndex))
                        {
                            continue; // Skip disabled columns
                        }
                        // filter out unwanted columns from body END

                        
                        object value = property.GetValue(item);
                        if (value is DateTime dateTimeValue)
                        {
                            // Convert DateTime to string in the desired format
                            string stringValue = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss");
                            worksheet.Cells[rowIndex, colCounter].Value = stringValue;
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, colCounter].Value = value;
                        }
                        worksheet.Cells[rowIndex, colCounter].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        colCounter++;
                    }

                    for (int i = 1; i <= properties.Length; i++) {
                        worksheet.Column(i).AutoFit(); 
                    }
                    rowIndex++;
                }
                // Add data rows END

                package.Save();
            }

            stream.Position = 0;
            return stream;
        }

        public async Task<Stream> ExportAllocation( string? sortValue="",string? sortType="",string? fromDate="",string? toDate="")
        {
            var result = await _bookingDataAccess.GetAllocation(fromDate, toDate);   
            var stream = ConvertIEnumerableToExcelStream( new string[0] , result, "alocation", 1,2,3);
            return stream;
        }
        public async Task<Stream> ExportPathology( string startDate, string endDate )
        {
            var result = await _bookingDataAccess.GetPathology(startDate, endDate);   
            var stream = ConvertIEnumerableToExcelStream(new string[0], result, "pathology", "Id","IsDeleted" ,"BookedDepartment", "NestedData","PostedBy","Status");
            return stream;
        }
        public async Task<Stream> ExportNonOperativeProcedure(string start,string end)
        {
            var result = await _bookingDataAccess.GetNonOPRequests(start, end);
            var stream = ConvertIEnumerableToExcelStream(new string[0], result, "pathology");
            return stream;
        }


        public async Task<EventFields> GetEventEquipmentsAndEmployees(int bookingId)
        {
            var equipments  = await _bookingDataAccess.GetEventEquipments(bookingId);
            var medicines  = await _bookingDataAccess.GetEventMedicines(bookingId);
            var materials  = await _bookingDataAccess.GetEventMaterials(bookingId);
            var employees   = await _bookingDataAccess.GetEventEmployees(bookingId);
            
            var surgeons    = employees.Where(employee => employee.EmployeeCategoryId==1);
            var nurses      = employees.Where(employee => employee.EmployeeCategoryId==2);

            var departmentIds = surgeons.Select(o => o.DepartmentID).Distinct();
            var allDepartments = await _bookingDataAccess.GetDepartments();
            var filteredDepartments = allDepartments.Where(o=>  departmentIds.Contains(o.Id));
            
            var fields = new EventFields();
            fields.Equipments = equipments;
            fields.Medicines = medicines;
            fields.Materials = materials;
            fields.Surgeons = surgeons;
            fields.Nurses = nurses;
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

            booking.MaterialsIdArray="["+booking.MaterialsIdArray+"]";
            booking.MedicineIdArray="["+booking .MedicineIdArray+"]";
            
            var result = await _bookingDataAccess.AddBooking(booking);
            return new Envelope<IEnumerable<int>>(true, "booking created", result);
        }

        public async Task<Envelope<IEnumerable<int>>> AddWaitingList(PostBookingModel booking)
        {
            // here there are no validations required. 
            booking.EmployeeIdArray="["+booking.EmployeeIdArray+"]";
            booking.EquipmentsIdArray="["+booking.EquipmentsIdArray+"]";
            booking.SurgeriesIdArray="["+booking.SurgeriesIdArray+"]";
            booking.MedicineIdArray="["+booking.MedicineIdArray+"]";
            booking.MaterialsIdArray="["+booking.MaterialsIdArray+"]";
            
            
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
                booking.SurgeriesIdArray="["+booking.SurgeriesIdArray+"]";
                booking.MedicineIdArray="["+booking.MedicineIdArray+"]";
                booking.MaterialsIdArray="["+booking.MaterialsIdArray+"]";
            

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
            booking.SurgeriesIdArray="["+booking.SurgeriesIdArray+"]";
            booking.MaterialsIdArray="["+booking.MaterialsIdArray+"]";
            booking.MedicineIdArray="["+booking .MedicineIdArray+"]";
            
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
            bookings = (IEnumerable<Bookings>) await PopulateBookingsWithSurgeries(bookings);
            // var surgeriesMapping = await _bookingDataAccess.GetEventSurgeries(bookingId);

            // return result;

            var result = new BookingsAndAllocations();
            result.Bookings = bookings;
            // result.Bookings.
            result.Allocations = filteredAllocations;
            return result;
        }
        public async Task<IEnumerable<Bookings>> GetBookingsForPathology(string? fromDate, string? toDate)
        {
            var result = await _bookingDataAccess.GetBookingsForPathology(fromDate, toDate);
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

        // CURRENTLY WORKING
        public async Task<BookingsAndAllocationsTheatres> SelectEventsTheatresAndDepartments(int? selectedOperationTheatreId , int departmentId, string? fromDate,string? toDate)
        {
            // sellects the unique operation theatre id those are allocated for the given departments
            // in accordance with given startDate and endDate
            
            // ALLOCATIONS SECTION START
            var allocations = await _bookingDataAccess.GetAllocation(fromDate, toDate);
            var filteredAllocationsWithDepartment = allocations.Where( o=> departmentId==o.AssignedDepartmentId);
            // ALLOCATIONS SECTION END

            // THEATRES SECTION START
            var allocatedOperationtheatres = filteredAllocationsWithDepartment.Select(o => o.OperationTheatreId).Distinct();

            
            // Find selected theatre 
            if (!allocatedOperationtheatres.Contains(selectedOperationTheatreId)){
                // if allocated theatre dosent contain selected theatre then select the first one as default 
                selectedOperationTheatreId = allocatedOperationtheatres.FirstOrDefault();
            }
            // THEATRES SECTION END

            // filter using selected ot id 
            var filteredAllocationsWithDepartmentAndOt = filteredAllocationsWithDepartment.Where( o=> ( selectedOperationTheatreId==o.OperationTheatreId)  ) ;

            // BOOKINGS SECTION START
            // selects bookings and allocations in accordance with given 
            // departmentId
            // operationTheatreId
            // fromDate
            // toDate
            var bookings = await _bookingDataAccess.GetBookingList(fromDate, toDate);
            bookings = bookings.Where(
                                        booking => (
                                                    (booking.OperationTheatreId == selectedOperationTheatreId) 
                                                    && 
                                                    ((departmentId == booking.BookedByDepartment) || (booking.StatusName=="BLOCKED") )
                                                 )
                                     );
            bookings = await PopulateBookingsWithSurgeries(bookings);
            // BOOKINGS SECTION END 


            var result = new BookingsAndAllocationsTheatres();
            result.Bookings = bookings;
            result.Theatres= allocatedOperationtheatres;
            result.Allocations = filteredAllocationsWithDepartmentAndOt;
            result.SelectedTheatre = selectedOperationTheatreId;            
            return result;
        }
        // CURRENTLY WORKING

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


        // PATHOLOGY Sample START
        public async Task<IEnumerable<Pathology>> GetPathology(string startDate, string endDate)
        {
            var result = await _bookingDataAccess.GetPathology(startDate, endDate);
            return result;
        }
        public async Task<PathologySampleSummary> GetPathologySummaryWithOperationId(int operationId)
        {
            var pathology = await _bookingDataAccess.GetPathologyWithOperationId(operationId);
            var firstRemovableDevice = pathology.FirstOrDefault();
            if (firstRemovableDevice != null)
            {
                int id = (int)firstRemovableDevice.Id;
                var samples = await _bookingDataAccess.GetPathologyDataWithId(id);
                PathologySampleSummary pathologySampleSummary = new PathologySampleSummary();

                pathologySampleSummary.pathology=pathology;
                pathologySampleSummary.samples=samples;
                return pathologySampleSummary;
                
            }
            
            PathologySampleSummary emptyPathologySampleSummary = new PathologySampleSummary();
            emptyPathologySampleSummary.pathology = Enumerable.Empty<Pathology>();
            emptyPathologySampleSummary.samples = Enumerable.Empty<PathologySample>();
            return emptyPathologySampleSummary;

            
        }
        public async Task<IEnumerable<PathologySample>> GetPathologyDataWithId(int id)
        {
            var result = await _bookingDataAccess.GetPathologyDataWithId(id);
            return result;
        }
        public async Task<IEnumerable<int>> PostPathology(Pathology Pathology)
        {
            var result = await _bookingDataAccess.PostPathology(Pathology);
            return result;
        }

        public async Task<IEnumerable<int>> PatchPathology(Pathology pathology)
        {
            var result = await _bookingDataAccess.PatchPathology(pathology);
            return result;
        }
        public async Task<IEnumerable<int>> DeletePathology(string idArray)
        {
            var result = await _bookingDataAccess.DeletePathology(idArray);
            return result;
        }
        // PATHOLOGY Sample END

        // Removable Devices START
        public async Task<IEnumerable<RemovableDevicesMain>> GetRemovableDevices()
        {
            var result = await _bookingDataAccess.GetRemovableDevices();
            return result;
        }

        public async Task<bool> UpdateIsRemovedStatus(int removableDeviceId, bool isRemoved)
        {
            var result = await _bookingDataAccess.UpdateIsRemovedStatus(removableDeviceId, isRemoved);
            return result;
        }

        public async Task<IEnumerable<RemovableDevices>> GetRemovableDevicesWithDate(string start, string end)
        {
            var result = await _bookingDataAccess.GetRemovableDevicesWithDate(start,end);
            return result;
        }
        public async Task<IEnumerable<RemovableDevicesSelcted>> GetRemovableDevicesSelected(int id)
        {
            var result = await _bookingDataAccess.GetRemovableDevicesSelected(id);
            return result;
        }
        public async Task<IEnumerable<int>> PostRemovableDevices(RemovableDevicesMain removableDevicesMain)
        {
            var result = await _bookingDataAccess.PostRemovableDevices(removableDevicesMain);
            return result;
        }
        public async Task<IEnumerable<int>> DeleteRemovableDeviceMain(string idArray)
        {
            var result = await _bookingDataAccess.DeleteRemovableDeviceMain(idArray);
            return result;
        }
        public async Task<IEnumerable<int>> EditRemovableDevices(RemovableDevicesMain removableDevicesMain)
        {
            var result = await _bookingDataAccess.EditRemovableDevices(removableDevicesMain);
            return result;
        }

        // Removable Devices START
        public async Task<RemovableDeviceSummary> GetRemovableDevicesSummaryWithId(int operationId)
        {
            var removableDeviceMain = await _bookingDataAccess.GetRemovableDevicesWithOperationId(operationId);
            var firstRemovableDevice = removableDeviceMain.FirstOrDefault();
            if (firstRemovableDevice != null)
            {
                int id = (int)firstRemovableDevice.Id;
                var removableDevicesSelected = await _bookingDataAccess.GetRemovableDevicesSelected(id);
                RemovableDeviceSummary removableDeviceSummary = new RemovableDeviceSummary();
                removableDeviceSummary.RemovableDevicesMain = removableDeviceMain;
                removableDeviceSummary.RemovableDevicesSelcted = removableDevicesSelected;
                return removableDeviceSummary;
            }
            
            RemovableDeviceSummary emptyRemovableDeviceSummary = new RemovableDeviceSummary();
            emptyRemovableDeviceSummary.RemovableDevicesMain = Enumerable.Empty<RemovableDevicesMain>();
            emptyRemovableDeviceSummary.RemovableDevicesSelcted = Enumerable.Empty<RemovableDevicesSelcted>();
            return emptyRemovableDeviceSummary;
        }



        #region NON-OP
        public async Task<Envelope<IEnumerable<NonOP>>> AddNonOPRequest(NonOP nonOP)
        {
            var result = await _bookingDataAccess.AddNonOPRequest(nonOP);
            return new Envelope<IEnumerable<NonOP>>(true, "data-update-success", result);
        }

        public async Task<IEnumerable<NonOP>> GetNonOPRequests(string start, string end)
        {
            var result = await _bookingDataAccess.GetNonOPRequests(start,end);
            return result;
        }

        public async Task<IEnumerable<NonOP>> GetNonOPRequestsummaryOperationId(int operationId)
        {
            var result = await _bookingDataAccess.GetNonOPRequestsWithOperationId(operationId);
            return result;
        }

        public async Task<Envelope<IEnumerable<NonOP>>> EditNonOPRequests(NonOP nonOP)
        {           
            var result = await _bookingDataAccess.EditNonOPRequests(nonOP);
            return new Envelope<IEnumerable<NonOP>>(true, "data-update-success", result);
        }
        public async Task<IEnumerable<NonOP>> DeleteNonOPRequests(string idArray)
        {
            var result = await _bookingDataAccess.DeleteNonOPRequests(idArray);
            return result;
        }
        #endregion


        // Post ot timings   START***
        public async Task<IEnumerable<BookingTime>> GetOTTimings(int bookingId)
        {
            var result = await _bookingDataAccess.GetOTTimings(bookingId);
            return result;
        }
        public async Task<IEnumerable<BookingTime>> PostOTTimings(BookingTime bookingTime)
        {
            var result = await _bookingDataAccess.PostOTTimings(bookingTime);
            return result;
        }
        public async Task<Envelope<int>> PostOTComplexEntryTiming(BookingTime bookingTime)
        {
            var result = await _bookingDataAccess.PostOTComplexEntryTiming(bookingTime);
            return new Envelope<int>(true, "Successsfully posted",0);
        }
        public async Task<Envelope<int>> PostPreOpTimings(BookingTime bookingTime)
        {
            // VALIDATION
            var _timings = await _bookingDataAccess.GetOTTimings((int)bookingTime.BookingId);
            var _firstElement = _timings.First();

            if(_firstElement.OtComplexEntry==null){
                return new Envelope<int>(false, "Please enter ot complex entry first",0);
            }
            // VALIDATION

            var result = await _bookingDataAccess.PostPreOpTimings(bookingTime);
            return new Envelope<int>(true, "Successsfully posted",0);
            // return result;
        }
        public async Task<Envelope<int>> PostInsideOTTimings(BookingTime bookingTime)
        {
            // VALIDATION
            var _timings = await _bookingDataAccess.GetOTTimings((int)bookingTime.BookingId);
            var _firstElement = _timings.First();

            if(_firstElement.PreOpEntryTime==null){
                return new Envelope<int>(false, "Please enter pre op timings first",0);
            }
            // VALIDATION   

            var result = await _bookingDataAccess.PostInsideOTTimings(bookingTime);
            return new Envelope<int>(true, "Successsfully posted",0);
        }
        public async Task<Envelope<int>> PostPostOpTimings(BookingTime bookingTime)
        {
            // VALIDATION
            var _timings = await _bookingDataAccess.GetOTTimings((int)bookingTime.BookingId);
            var _firstElement = _timings.First();

            if(_firstElement.OtEntryTime ==null){
                return new Envelope<int>(false, "Please enter ot entry timings first",0);
            }
            // VALIDATION   

            var result = await _bookingDataAccess.PostPostOpTimings(bookingTime);
            return new Envelope<int>(true, "Successsfully posted",0);
        }


        // DASHBOARD REGION START
        #region DASHBOARD-TODAY
        public async Task<IEnumerable<DashbordOTGroup>> GetTodaysOtStatuses()
        {
            
            var dataRes = new DashboardData();
            
            var result = await _bookingDataAccess.GetTodaysOtStatuses();

            List<DashbordOTGroup> groupedDataList = result
            .GroupBy(o => o.OperationTheatreId)
            .Select(group => new DashbordOTGroup
            {
                OperationTheatreId = group.Key,
                OperationTheatreName = group.First().OperationTheatreName,
                OperationsList = group.ToList()
            })
            .ToList();

            // Perform operations on the categorized data
            foreach (var group in groupedDataList)
            {
                // Access the properties of each group
                int? operationTheatreId = group.OperationTheatreId;
                string operationTheatreName = group.OperationTheatreName;
                
                int todaysTotal=0;
                int todaysCompleted=0;

                int complexEntryCount=0;
                int preOpCount=0;
                int postOpCount=0;
                int inOtCount=0;
                
                
                List<DashboardOperation> operationsList = group.OperationsList;
                foreach (var operation in operationsList)
                {
                    todaysTotal++;
                    string complexLocation="SCHEDULED";
                    if(operation.OtComplexEntry!=null){
                        complexLocation="IN_COMPLEX_RECEPTION";
                        complexEntryCount++;
                    }
                    if(operation.PreOpEntryTime!=null){
                        complexLocation="PRE_OP";
                        preOpCount++;
                    }
                    if(operation.OtEntryTime!=null){
                        complexLocation="IN_OT";
                        inOtCount++;
                    }
                    if(operation.PostOpEntryTime!=null){
                        complexLocation="POST_OP";
                        postOpCount++;
                    }
                    if(operation.PostOpExitTime!=null){
                        complexLocation="EXIT";
                    }
                    operation.ComplexLocation=complexLocation;
                }
                
                group.TotalCases = todaysTotal;
                group.CompletedCases = postOpCount;
                group.InComplexCasesCount = complexEntryCount;
                group.InPreOpCasesCount = preOpCount;
                group.InPostOpCasesCount = postOpCount;
                group.inOtCasesCount = inOtCount;
                group.pendingCasesCount = todaysTotal-postOpCount;
            }
            
            // groupedDataList.OperationsList=operationsList;
            // dataRes.OtStatuses = groupedDataList;
            return groupedDataList;
        }
        #endregion
        // DASHBOARD REGION END


    }
}
