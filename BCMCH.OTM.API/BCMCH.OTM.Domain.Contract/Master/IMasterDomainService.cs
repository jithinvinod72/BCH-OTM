using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        // Task<IEnumerable<Employee>> GetEmployees(string _searchOption , string _departmentArray);
        Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray,  int pageNumber, int rowsOfPage);
        Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId, string? _fromDate);
        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<AllMasters> GetMasters();

        Task<IEnumerable<GetAllocationModel>> GetAllocation(string startDate,string endDate);
        Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
    }
}