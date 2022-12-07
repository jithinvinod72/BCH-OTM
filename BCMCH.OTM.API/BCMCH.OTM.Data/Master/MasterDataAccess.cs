using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using System.Data;


namespace BCMCH.OTM.Data.Master
{
    public class MasterDataAccess : IMasterDataAccess
    {
        #region PRIVATE
        private readonly ISqlDbHelper _sqlHelper;
        #endregion

        #region CONSTRUCTOR
        public MasterDataAccess(ISqlDbHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        #endregion

        #region PUBLIC
        public async Task<IEnumerable<Equipments>> EquipmentsDetails()
        {
            const string StoredProcedure = "[OTM].[SelectEquipments]";
            var SqlParameters = new DynamicParameters();

            var result= await _sqlHelper.QueryAsync<Equipments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        
        public async Task<IEnumerable<Departments>> DepartmentDetails()
        {
            const string StoredProcedure = "[OTM].[SelectDepartments]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Search", "");
            SqlParameters.Add("@pageNumber", 1);
            SqlParameters.Add("@RowsOfPage", 100);

            var result= await _sqlHelper.QueryAsync<Departments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        
        }
        public async Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList()
        {
            const string StoredProcedure = "[OTM].[SelectAnaesthesiaList]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Search", "");

            var result= await _sqlHelper.QueryAsync<Anaesthesia>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeList(string _searchOption , string _departmentArray )
        {
            const string StoredProcedure = "[OTM].[SelectEmployeesWithDepartmentsMapping]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Search", _searchOption);
            SqlParameters.Add("@DepartmentsToFetchFrom", _departmentArray);
            var result= await _sqlHelper.QueryAsync<Employee>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId=0, string? _fromDate="")
        {
            // if department id is 0 fetches all department datas
            // else fetches with department id
            const string StoredProcedure = "[OTM].[SelectOperationTheatreAllocation]";
            var SqlParameters = new DynamicParameters();
            
            SqlParameters.Add("@DepartmentId", _departmentId);
            SqlParameters.Add("@FromDate", _fromDate );

            var result= await _sqlHelper.QueryAsync<OperationTheatreAllocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatreList()
        {
            const string StoredProcedure = "[OTM].[SelectOperationTheatres]";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<OperationTheatre>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="")
        {
            const string StoredProcedure = "[OTM].[SelectSurgeries]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PageNumber", _pageNumber);
            SqlParameters.Add("@RowsOfPage", _rowsPerPage );
            SqlParameters.Add("@Search", _searchKeyword );

            var result= await _sqlHelper.QueryAsync<Surgery>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            const string StoredProcedure = "[OTM].[InsertAllocation]";
            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@OperationTheatreId"     , _allocation.OperationTheatreId );
            SqlParameters.Add("@AssignedDepartmentId"   , _allocation.AssignedDepartmentId );
            SqlParameters.Add("@StartDate"              , _allocation.StartDate );
            SqlParameters.Add("@EndDate"                , _allocation.EndDate );
            SqlParameters.Add("@ModifiedBy"             , _allocation.ModifiedBy );

            var result= await _sqlHelper.QueryAsync<Allocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        
        #endregion
        
    }
}
