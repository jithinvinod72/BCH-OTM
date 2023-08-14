using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Data.Contract.Master
{
    public interface IMasterDataAccess
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string departmentArray);
        

        // role section 
        Task<IEnumerable<UserAndHisRole>> GetOTUserRole(int employeeId);
        Task<IEnumerable<UserAndHisRole>> GetUsersAndRoles();
        Task<IEnumerable<UserResources>> GetOTRolePermissions(int? roleId);
        Task<IEnumerable<AvailableRoles>> GetOTRoles();
        Task<IEnumerable<int>> PostNewOTUser(int EmployeeId, int roleId);
        Task<IEnumerable<int>> UpdateOTUser(UserAndHisRole userAndHisRole);
        Task<IEnumerable<int>> DeleteOTUser(string EmployeeIdList);
        Task<IEnumerable<int>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights);
        Task<IEnumerable<Resources>> GetOTResources();
        Task<IEnumerable<int>> UpdateRolePermissions(PostAdminRolesAndRights otAdminAndRights);


        Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode);
        Task<IEnumerable<Employee>> GetEmployeesWithCategoryId(int emplyoeeCategoryId);

        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<IEnumerable<GetAllocation>> GetAllocations(string startDate, string endDate);
        Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<Allocation>> EditAllocation(Allocation _allocation);
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

        //validation- ot allocation
        Task<IEnumerable<GetAllocation>> CheckAllocationByOperationThearter(string startDate, string endDate, int operationTheatreId);
        Task<IEnumerable<NonOperativeProcedureList>> GetNonOperativeProceduresList();
        

        
    }
}
