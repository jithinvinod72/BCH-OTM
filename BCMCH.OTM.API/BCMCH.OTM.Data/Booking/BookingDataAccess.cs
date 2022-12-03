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
    }
}
