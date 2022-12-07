using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.Data.Contract.Master
{
    public interface IMasterDataAccess
    {
        Task<IEnumerable<Departments>> DepartmentDetails();
        Task<IEnumerable<Equipments>> EquipmentsDetails();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployeeList(string _searchOption , string _departmentArray);
        Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId, string? _fromDate);
        Task<IEnumerable<OperationTheatre>> GetOperationTheatreList();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
    }
}
