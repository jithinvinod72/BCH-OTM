using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using System.Data;
using System.Data.SqlClient;

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
        public async Task<IEnumerable<SpecialEquipments>> SpecialEquipmentsDetails()
        {
            const string StoredProcedure = "[OTM].[GET_Equipments]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Search", "a");
            SqlParameters.Add("@pageNumber", 1);
            SqlParameters.Add("@RowsOfPage", 10);

            var result= await _sqlHelper.QueryAsync<SpecialEquipments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Departments>> DepartmentDetails()
        {
            const string StoredProcedure = "[OTM].[GET_Departments]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@Search", "");
            SqlParameters.Add("@pageNumber", 1);
            SqlParameters.Add("@RowsOfPage", 10);

            var result= await _sqlHelper.QueryAsync<Departments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        
        }
        #endregion
    }
}
