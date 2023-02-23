using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Data.Contract.Master
{
    public interface IMasterDataAccess
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray, int pageNumber, int rowsOfPage );
        Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode);
        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<IEnumerable<GetAllocationModel>> GetAllocations(string startDate, string endDate);
        Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<int>> DeleteAllocations(string allocationIds);
        Task<IEnumerable<DateTime>> GetDateToday();
        

        // QUESTION SECTION START
         Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question);
         Task<IEnumerable<string>> PostQuestionType(string name,string label);
         Task<IEnumerable<string>> PostOtStages(string name,string label);
         Task<IEnumerable<GetQuestions>> GetFormQuestions();
         Task<IEnumerable<FormSections>> GetFormSections();
         Task<IEnumerable<FormSections>> GetFormQuestionType();
         // QUESTION SECTION END
         Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer);
         Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId);
    }
}
