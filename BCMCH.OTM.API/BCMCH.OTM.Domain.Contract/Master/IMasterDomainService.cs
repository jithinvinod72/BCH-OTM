using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Infrastucture.Generic;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Medicines>> GetMedicines();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string departmentArray);
        Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode);
        Task<IEnumerable<Employee>> GetEmployeesWithCategoryId(int emplyoeeCategoryId);

        // OT ROLE START
        Task<UserRole> GetOTUserRole(int employeeId);
        Task<IEnumerable<UserAndHisRole>> GetUsersAndRoles();
        Task<IEnumerable<AvailableRoles>> GetOTRoles();
        Task<IEnumerable<Resources>> GetOTResources();
        Task<Envelope<IEnumerable<int>>> PostNewOTUser(UserAndHisRole userAndHisRole);
        Task<IEnumerable<int>> UpdateOTUser(UserAndHisRole UserAndHisRole);
        Task<IEnumerable<int>> DeleteOTUser(string EmployeeIdList);
        Task<Envelope<IEnumerable<int>>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights);
        Task<IEnumerable<UserResources>> GetOTRolePermissions(int? roleId);
        Task<IEnumerable<int>> UpdateRolePermissions(PostAdminRolesAndRights otAdminAndRights);
        Task<IEnumerable<int>> DeleteRolesAndPermissions(string roleIdList);

        // OT ROLE END

        Task<IEnumerable<OperationTheatre>> GetOperationTheatres();
        Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="");
        Task<AllMasters> GetMasters();
        Task<IEnumerable<GetAllocation>> GetAllAllocations(string startDate, string endDate);
        Task<IEnumerable<GetAllocationGroupped>> GetAllocationsGroup(string startDate, string endDate);
        
        // Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation);
        // Task<IEnumerable<int>> PostAllocation(Allocation _allocation);
        Task<Envelope<IEnumerable<GetAllocation>>> PostAllocation(Allocation _allocation);
        Task<IEnumerable<int>> EditAllocation(Allocation _allocation);
        // Task<Envelope<IEnumerable<Allocation>>> PostAllocationInARange(AllocateInRange _allocation);
        Task<Envelope<IEnumerable<GetAllocation>>> PostAllocationInARange(AllocateInRange _allocation);
        Task<IEnumerable<int>> DeleteAllocations(string allocationIds);
        Task<DateTime>GetDateToday();

        // QUESTION SECTIION START
        Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question);
        Task<IEnumerable<string>> DisableQuestions(int id);
        Task<IEnumerable<string>> PostQuestionType(string name,string label);
        Task<IEnumerable<string>>  PostOtStages(string name,string label);
        Task<IEnumerable<GetQuestions>> GetFormQuestions(int otStageId, string accessibleTo);
        Task<IEnumerable<GetQuestions>> GetAllFormQuestions();
        // QUESTION SECTIION END
        Task<FormMasters> GetFormMasters();
        Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer);
        Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId);
        Task<IEnumerable<NonOperativeProcedureList>> GetNonOperativeProceduresList();

    }
}