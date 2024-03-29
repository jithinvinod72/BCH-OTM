﻿using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
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
        public async Task<IEnumerable<Equipments>> GetEquipments()
        {
            var result = await _masterDataAccess.GetEquipments();
            return result;
        }
        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            var result = await _masterDataAccess.GetDepartments();
            return result;
        }
        
        public async Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList()
        {
            var result = await _masterDataAccess.GetAnaesthesiaList();
            return result;
        }
        
        public async Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray,  int pageNumber, int rowsOfPage)
        {
            searchOption.Replace(" ", "%");
            searchOption = "%"+searchOption+"%";
            // replace space with % for sp and 
            // adds % as first and last charecter
            // GetEmployees(string searchOption , string departmentArray, int pageNumber, int rowsOfPage )

            var result = await _masterDataAccess.GetEmployees(searchOption ,departmentArray, pageNumber,rowsOfPage);
            return result;
        }
        public async Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId, string? _fromDate)
        {
            var result = await _masterDataAccess.GetOperationTheatreAllocations(_departmentId, _fromDate);
            return result;
        }
        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatres()
        {
            var result = await _masterDataAccess.GetOperationTheatres();
            return result;
        }

        public async  Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="")
        {
            _searchKeyword.Replace(" ", "%");
            _searchKeyword = "%"+_searchKeyword+"%";
            var result = await _masterDataAccess.GetSurgeryList(_pageNumber, _rowsPerPage, _searchKeyword);
            return result;
        }
        public async Task<AllMasters> GetMasters()
        {
            AllMasters allMasters = new AllMasters();
            
            allMasters.EquipmentList = await _masterDataAccess.GetEquipments();
            allMasters.AnaesthetistList = await _masterDataAccess.GetEmployees("%%", "[2]", 1, 100 ); // 2 is the department of anaesthetists
            allMasters.OperationTheatreList = await _masterDataAccess.GetOperationTheatres();
            allMasters.AnaesthesiaList = await _masterDataAccess.GetAnaesthesiaList();
            allMasters.DepartmentsList = await _masterDataAccess.GetDepartments();
            allMasters.DateTimeToday = await GetDateToday();

            return allMasters;
        }

        public async Task<IEnumerable<GetAllocationModel>> GetAllocation(int departmentId,string startDate, string endDate)
        {
            var result = await _masterDataAccess.GetAllocation(departmentId, startDate, endDate);
            return result;
        }
            
        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            var result = await _masterDataAccess.PostAllocation(_allocation);
            return result;
        }
        public async Task<DateTime>GetDateToday()
        {
            var result = await _masterDataAccess.GetDateToday();
            return result.ElementAt(0);
        }
        
        #endregion
        
    }
}
