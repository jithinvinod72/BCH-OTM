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
        public async Task<IEnumerable<Equipments>> EquipmentsDetails()
        {
            var result = await _masterDataAccess.EquipmentsDetails();
            return result;
        }
        public async Task<IEnumerable<Departments>> DepartmentDetails()
        {
            var result = await _masterDataAccess.DepartmentDetails();
            return result;
        }
        
        public async Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList()
        {
            var result = await _masterDataAccess.GetAnaesthesiaList();
            return result;
        }
        
        public async Task<IEnumerable<Employee>> GetEmployeeList(string _searchOption , string _departmentArray)
        {
            var result = await _masterDataAccess.GetEmployeeList(_searchOption ,_departmentArray);
            return result;
        }
        public async Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId, string? _fromDate)
        {
            var result = await _masterDataAccess.GetOperationTheatreAllocations(_departmentId, _fromDate);
            return result;
        }
        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatreList()
        {
            var result = await _masterDataAccess.GetOperationTheatreList();
            return result;
        }

        public async  Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="")
        {
            var result = await _masterDataAccess.GetSurgeryList(_pageNumber, _rowsPerPage, _searchKeyword);
            return result;
        }
        #endregion
        
    }
}
