using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray,  int pageNumber, int rowsOfPage);
        Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode);
        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<AllMasters> GetMasters();
        Task<IEnumerable<GetAllocationModel>> GetAllocations(string startDate, string endDate);
        Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<int>> PostAllocationInARange(AllocateInRange _allocation);
        Task<IEnumerable<int>> DeleteAllocations(string allocationIds);
        Task<DateTime>GetDateToday();

        // QUESTION SECTIION START
        Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question);
        Task<IEnumerable<string>> PostQuestionType(string questionType);
        Task<IEnumerable<string>> PostFormSections(string section);
        Task<IEnumerable<GetQuestions>> GetFormQuestions();
        // QUESTION SECTIION END
        Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer);
        Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId);
    }
}