using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
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
        public async Task<IEnumerable<SpecialEquipments>> SpecialEquipmentsDetails()
        {
            const string StoredProcedure = "GetAllCompanyDetails";

            return await _sqlHelper.QueryAsync<SpecialEquipments>(StoredProcedure, null, CommandType.StoredProcedure);

        }
        #endregion
    }
}
