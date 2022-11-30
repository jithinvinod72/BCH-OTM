using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<Departments>> DepartmentDetails();
        Task<IEnumerable<Equipments>> EquipmentsDetails();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployeeList(string _searchOption , string _departmentArray);
        Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId, string? _fromDate);
        Task<IEnumerable<OperationTheatre>> GetOperationTheatreList();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
    }
}