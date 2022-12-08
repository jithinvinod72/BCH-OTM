using BCMCH.OTM.Data.Contract.Booking;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using BCMCH.OTM.API.Shared.Booking;
using System.Data;

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

        public async Task<IEnumerable<Bookings>> GetBookingList(int _operationTheatreId, string? _fromDate,string? _toDate)
        {
            const string StoredProcedure = "[OTM].[SelectBookings]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@FromDate", _fromDate);
            SqlParameters.Add("@ToDate",   _toDate );
            SqlParameters.Add("@OperationTheatreId", _operationTheatreId );

            var result= await _sqlHelper.QueryAsync<Bookings>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<PostBookingModel>> PostBooking(PostBookingModel _booking)
        {
            const string StoredProcedure = "[OTM].[InsertBooking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", _booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId",   _booking.DoctorId );
            SqlParameters.Add("@AnaesthetistId", _booking.AnaesthetistId );

            SqlParameters.Add("@StatusId", _booking.StatusId );
            SqlParameters.Add("@AnaesthesiaTypeId", _booking.AnaesthesiaTypeId );
            SqlParameters.Add("@SurgeryId", _booking.SurgeryId );

            SqlParameters.Add("@RegistrationNo", _booking.RegistrationNo );
            SqlParameters.Add("@StartDate", _booking.StartDate );
            SqlParameters.Add("@EndDate", _booking.EndDate );
            SqlParameters.Add("@Duration", _booking.Duration );
            SqlParameters.Add("@InstructionToNurse", _booking.InstructionToNurse );
            SqlParameters.Add("@InstructionToAnaesthetist", _booking.InstructionToAnaesthetist );
            SqlParameters.Add("@InstructionToOperationTeatrePersons", _booking.InstructionToOperationTeatrePersons );

            SqlParameters.Add("@RequestForSpecialMeterial", _booking.RequestForSpecialMeterial );
            SqlParameters.Add("@DepartmentId", _booking.DepartmentId );
            SqlParameters.Add("@Type", _booking.Type );
            SqlParameters.Add("@EmployeeIdArray", _booking.EmployeeIdArray );
            SqlParameters.Add("@EquipmentsIdArray", _booking.EquipmentsIdArray );

            var result= await _sqlHelper.QueryAsync<PostBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        public async Task<IEnumerable<PostBookingModel>> UpdateBooking(PostBookingModel _booking)
        {
            const string StoredProcedure = "[OTM].[UpdateBooking]";
            var SqlParameters = new DynamicParameters();
            // SqlParameters.Add("@Id", _booking.Id);
            SqlParameters.Add("@OperationTheatreId", _booking.OperationTheatreId);
            SqlParameters.Add("@DoctorId", _booking.DoctorId);
            SqlParameters.Add("@AnaesthetistId", _booking.AnaesthetistId);

            SqlParameters.Add("@StatusId", _booking.StatusId);
            SqlParameters.Add("@AnaesthesiaTypeId", _booking.AnaesthesiaTypeId);
            SqlParameters.Add("@SurgeryId", _booking.SurgeryId);

            SqlParameters.Add("@RegistrationNo", _booking.RegistrationNo);
            SqlParameters.Add("@StartDate", _booking.StartDate);
            SqlParameters.Add("@EndDate", _booking.EndDate);
            SqlParameters.Add("@Duration", _booking.Duration);
            SqlParameters.Add("@InstructionToNurse", _booking.InstructionToNurse);
            SqlParameters.Add("@InstructionToAnaesthetist", _booking.InstructionToAnaesthetist);
            SqlParameters.Add("@InstructionToOperationTeatrePersons", _booking.InstructionToOperationTeatrePersons);

            SqlParameters.Add("@RequestForSpecialMeterial", _booking.RequestForSpecialMeterial);
            SqlParameters.Add("@DepartmentId", _booking.DepartmentId);
            SqlParameters.Add("@Type", _booking.Type);
            SqlParameters.Add("@EmployeeIdArray", _booking.EmployeeIdArray);
            SqlParameters.Add("@EquipmentsIdArray", _booking.EquipmentsIdArray);

            var result = await _sqlHelper.QueryAsync<PostBookingModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        public async Task<int> IsOperationTheatreAllocated(int _operationTheatreId,int _departmentId , DateTime _startDate, DateTime _endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreAllocated]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@@departmentId",_departmentId);
            SqlParameters.Add("@operationTheatreId",_operationTheatreId);
            SqlParameters.Add("@StartDateToSearch", _startDate);
            SqlParameters.Add("@EndDateToSearch",   _endDate );
            var result= await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> IsOperationTheatreBloked(int _operationTheatreId, DateTime _startDate, DateTime _endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreBloked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@operationTheatreId",   _operationTheatreId );
            SqlParameters.Add("@StartDateToSearch", _startDate);
            SqlParameters.Add("@EndDateToSearch",   _endDate);
            var result= await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> IsOperationTheatreBooked(int _operationTheatreId, DateTime _startDate, DateTime _endDate)
        {
            const string StoredProcedure = "[OTM].[IsOperationTheatreBooked]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@operationTheatreId",_operationTheatreId );
            SqlParameters.Add("@StartDateToSearch", _startDate);
            SqlParameters.Add("@EndDateToSearch",   _endDate);
            var result= await _sqlHelper.ExecuteAsync(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        



        #region BLOCKING
        public async Task<IEnumerable<Blocking>> PostBlocking(Blocking _blocking)
        {
            const string StoredProcedure = "[OTM].[InsertBlocking]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@OperationTheatreId", _blocking.OperationTheatreId);
            SqlParameters.Add("@StatusId",   3 );
            SqlParameters.Add("@StartDate", _blocking.StartDate );
            SqlParameters.Add("@EndDate", _blocking.EndDate);
            SqlParameters.Add("@Duration",   _blocking.Duration );
            SqlParameters.Add("@ModifiedBy", _blocking.ModifiedBy );
            SqlParameters.Add("@Type", "BLOCKING" );

            var result= await _sqlHelper.QueryAsync<Blocking>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        #endregion

    }
    
}
