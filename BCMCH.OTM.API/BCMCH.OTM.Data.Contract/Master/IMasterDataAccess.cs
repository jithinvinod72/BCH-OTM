﻿using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;

namespace BCMCH.OTM.Data.Contract.Master
{
    public interface IMasterDataAccess
    {
        Task<IEnumerable<Departments>> GetDepartments();
        Task<IEnumerable<Equipments>> GetEquipments();
        Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList();
        Task<IEnumerable<Employee>> GetEmployees(string departmentArray);
        Task<IEnumerable<UserRoleDetails>> GetOTUserRole(int employeeId);
        Task<IEnumerable<UserResources>> GetOTRolePermissions(int? roleId);
        Task<IEnumerable<AvailableRoles>> GetOTRoles();
        Task<IEnumerable<int>> PostNewOTUser(int userId, int roleId);
        Task<IEnumerable<int>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights);
        Task<IEnumerable<Resources>> GetOTResources();
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
        Task<IEnumerable<OTValidation>> CheckAllocationByOperationThearter(string startDate, string endDate, int operationId);
        Task<IEnumerable<NonOperativeProcedureList>> GetNonOperativeProceduresList();
        

        
    }
}
