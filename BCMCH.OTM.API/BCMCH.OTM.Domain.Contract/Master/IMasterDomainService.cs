using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Infrastucture.Generic;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string departmentArray);
        Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode);

        // OT ROLE START
        Task<UserRole> GetOTUserRole(int employeeId);
        Task<IEnumerable<AvailableRoles>> GetOTRoles();
        Task<IEnumerable<Resources>> GetOTResources();
        Task<IEnumerable<int>> PostNewOTUser(UserRoleDetails userRoleDetails);
        Task<IEnumerable<int>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights);
        // OT ROLE END

        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<AllMasters> GetMasters();
        Task<IEnumerable<GetAllocationModel>> GetAllocations(string startDate, string endDate);
        // Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<int>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<int>> PostAllocationInARange(AllocateInRange _allocation);
        Task<IEnumerable<int>> DeleteAllocations(string allocationIds);
        Task<DateTime>GetDateToday();

        // QUESTION SECTIION START
        Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question);
        Task<IEnumerable<string>> PostQuestionType(string name,string label);
        Task<IEnumerable<string>>  PostOtStages(string name,string label);
        Task<IEnumerable<GetQuestions>> GetFormQuestions(int otStageId, string accessibleTo);
        // QUESTION SECTIION END
        Task<FormMasters> GetFormMasters();
        Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer);
        Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId);
        Task<IEnumerable<NonOperativeProcedureList>> GetNonOperativeProceduresList();

    }
}