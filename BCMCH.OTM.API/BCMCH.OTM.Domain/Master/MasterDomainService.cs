using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Domain.Contract.Master;

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
        }
        #endregion

        #region PUBLIC
        public async Task<IEnumerable<SpecialEquipments>> SpecialEquipmentsDetails()
        {
            var result = await _masterDataAccess.SpecialEquipmentsDetails();
            return result;
        }
        public async Task<IEnumerable<Departments>> DepartmentDetails()
        {
            var result = await _masterDataAccess.DepartmentDetails();
            return result;
        }
        #endregion
    }
}
