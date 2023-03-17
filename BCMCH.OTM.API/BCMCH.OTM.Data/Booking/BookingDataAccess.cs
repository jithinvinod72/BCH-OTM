using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using BCMCH.OTM.API.Shared.Booking;
using System.Data;
using System.Data.Common;
using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Data.Booking
{
    public class BookingDataAccess : IBookingDataAccess
    {
        #region PRIVATE
        private readonly ISqlDbHelper _sqlHelper;
        #endregion

        #region CONSTRUCTOR
        public BookingDataAccess(ISqlDbHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        #endregion

        public async Task<IEnumerable<Bookings>> GetBookingList(string? fromDate,string? toDate)
        {
            const string StoredProcedure = "[OTM].[SelectBookings]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@FromDate", fromDate);
            SqlParameters.Add("@ToDate",   toDate );
            var result= await _sqlHelper.QueryAsync<Bookings>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<int>> AddBooking(PostBookingModel booking)
        {
            const string StoredProcedure = "[OTM].[InsertBooking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId",   booking.DoctorId );
            SqlParameters.Add("@AnaesthetistId", booking.AnaesthetistId );

            SqlParameters.Add("@StatusId", booking.StatusId );
            SqlParameters.Add("@AnaesthesiaTypeId", booking.AnaesthesiaTypeId );
            SqlParameters.Add("@SurgeryId", booking.SurgeryId );

            SqlParameters.Add("@RegistrationNo", booking.RegistrationNo );
            SqlParameters.Add("@StartDate", booking.StartDate );
            SqlParameters.Add("@EndDate", booking.EndDate );
            SqlParameters.Add("@Duration", booking.Duration );
            SqlParameters.Add("@InstructionToNurse", booking.InstructionToNurse );
            SqlParameters.Add("@InstructionToAnaesthetist", booking.InstructionToAnaesthetist );
            SqlParameters.Add("@InstructionToOperationTeatrePersons", booking.InstructionToOperationTeatrePersons );

            SqlParameters.Add("@RequestForSpecialMeterial", booking.RequestForSpecialMeterial );
            SqlParameters.Add("@DepartmentId", booking.DepartmentId );
            // SqlParameters.Add("@Type", booking.Type );
            SqlParameters.Add("@EmployeeIdArray", booking.EmployeeIdArray );
            SqlParameters.Add("@EquipmentsIdArray", booking.EquipmentsIdArray );

            var result= await _sqlHelper.QueryAsync<int>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<GetAllocationModel>> GetAllocation(string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[SelectAllocation]";
            var SqlParameters = new DynamicParameters();
            // SqlParameters.Add("@DeartmentId"    , departmentId );
            SqlParameters.Add("@StartDate"      , startDate );
            SqlParameters.Add("@EndDate"        , endDate );
            var result= await _sqlHelper.QueryAsync<GetAllocationModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Equipments>> GetEventEquipments(int bookingId)
        {
            string Query =@"
                            SELECT 
                                [EquipmentsMaster].Id ,
                                [Name],
                                [Description]
                            FROM [OTM].[EquipmentsMapping]
                            LEFT JOIN  
                                [OTM].[EquipmentsMaster] ON [EquipmentId] =[OTM].[EquipmentsMaster].[Id]
                            WHERE 
                                BookingId=@bookingId" ;

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId"    , bookingId );
            var result= await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEventEmployees(int bookingId)
        {
            // used to fetch the employees with their booking Id
            string Query =  @"
                                SELECT 
                                    EmployeeId,
                                    FirstName,
                                    MiddleName,
                                    LastName,
                                    DepartmentID 
                                FROM  
                                    [OTM].[EmployeeMapping]
                                LEFT JOIN 
                                    HR.[Employees] ON OTM.EmployeeMapping.EmployeeId =HR.[Employees].Id
                                WHERE 
                                    BookingId=@bookingId
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@bookingId"    , bookingId.ToString() );
            var result= await _sqlHelper.QueryAsync<Employee>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            string Query =  @"
                                SELECT 
                                        [Id],
                                        [Code],
                                        [DivisionId],
                                        [TypeCode],
                                        [Name]
                                FROM 
                                    dbo.Departments
                            ";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<Departments>(Query, SqlParameters, CommandType.Text);
            return result;
        }




        public async Task<IEnumerable<UpdateBookingModel>> UpdateBooking(UpdateBookingModel booking)
        {
            const string StoredProcedure = "[OTM].[UpdateBooking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Id", booking.Id);
            SqlParameters.Add("@OperationTheatreId", booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId", booking.DoctorId);
            SqlParameters.Add("@AnaesthetistId", booking.AnaesthetistId);

            SqlParameters.Add("@StatusId", booking.StatusId);
            SqlParameters.Add("@AnaesthesiaTypeId", booking.AnaesthesiaTypeId);
            SqlParameters.Add("@SurgeryId", booking.SurgeryId);

            SqlParameters.Add("@RegistrationNo", booking.RegistrationNo);
            SqlParameters.Add("@StartDate", booking.StartDate);
            SqlParameters.Add("@EndDate", booking.EndDate);
            SqlParameters.Add("@Duration", booking.Duration);
            SqlParameters.Add("@InstructionToNurse", booking.InstructionToNurse);
            SqlParameters.Add("@InstructionToAnaesthetist", booking.InstructionToAnaesthetist);
            SqlParameters.Add("@InstructionToOperationTeatrePersons", booking.InstructionToOperationTeatrePersons);

            SqlParameters.Add("@RequestForSpecialMeterial", booking.RequestForSpecialMeterial);
            SqlParameters.Add("@DepartmentId", booking.DepartmentId);
            SqlParameters.Add("@Type", booking.Type);
            SqlParameters.Add("@EmployeeIdArray", booking.EmployeeIdArray);
            SqlParameters.Add("@EquipmentsIdArray", booking.EquipmentsIdArray);

            var result = await _sqlHelper.QueryAsync<UpdateBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        public async Task<int> IsOperationTheatreAllocated(int operationTheatreId,int departmentId , string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreAllocated]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch",   endDate );
            SqlParameters.Add("@operationTheatreId",operationTheatreId);
            SqlParameters.Add("@departmentId",departmentId);
            var result = await _sqlHelper.QueryAsync<UpdateBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            // the result has Ienumerable type 
            // what we do is we simply return the count/length of the ienumerable 
            // if the count is less than 0 then the ot is not allocated for the department 
            // if the count is greater than 0 then the ot is allocated for the department 

            return result.Count();
        }
        public async Task<int> IsOperationTheatreBloked(int operationTheatreId, string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreBloked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@operationTheatreId",   operationTheatreId );
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch",   endDate);
            var result= await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> IsOperationTheatreBooked(int bookingIdToExcludeFromSearch, int operationTheatreId, string startDate, string endDate)
        {
            // bookingIdToExcludeFromSearch is used in updation , 
            // for updation we dont have to check the time slots with current operation id 
            // for inserting bookingIdToExcludeFromSearch is 0
            const string StoredProcedure = "[OTM].[IsOperationTheatreBooked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@BookingIdToExclude",bookingIdToExcludeFromSearch );
            SqlParameters.Add("@operationTheatreId",operationTheatreId );
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch",   endDate);
            var result= await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);

            return result;
        }
        

        public async Task<IEnumerable<Bookings>> DeleteBooking(string IdArray)
        {
            string Query =  @"
                                UPDATE 
                                    [OTM].[Bookings]
                                SET 
                                    [IsDeleted] = 1
                                WHERE 
                                    Id IN (SELECT value FROM OPENJSON(@IdArray))
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@IdArray"    , IdArray );
            var result= await _sqlHelper.QueryAsync<Bookings>(Query, SqlParameters, CommandType.Text);
            return result;
        }



        #region BLOCKING
        public async Task<IEnumerable<Blocking>> AddBlocking(Blocking blocking)
        {
            const string StoredProcedure = "[OTM].[InsertBlocking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", blocking.OperationTheatreId);
            SqlParameters.Add("@StatusId",   3 );
            SqlParameters.Add("@StartDate", blocking.StartDate );
            SqlParameters.Add("@EndDate", blocking.EndDate);
            SqlParameters.Add("@Duration",   blocking.Duration );
            SqlParameters.Add("@ModifiedBy", blocking.ModifiedBy );
            
            var result= await _sqlHelper.QueryAsync<Blocking>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Patient>> GetPatientData(string registrationNo)
        {
            const string StoredProcedure = "[OTM].[SelectPatientDetails]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@registrationNo", registrationNo);
            var result= await _sqlHelper.QueryAsync<Patient>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        #endregion

        

    }
    
}
