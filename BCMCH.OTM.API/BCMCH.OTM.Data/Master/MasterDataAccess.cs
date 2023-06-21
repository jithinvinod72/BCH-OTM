﻿using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using System.Data;
using System.Numerics;
using BCMCH.OTM.API.Shared.Booking;

namespace BCMCH.OTM.Data.Master
{
    public class MasterDataAccess : IMasterDataAccess
    {
        #region PRIVATE
        private readonly ISqlDbHelper _sqlHelper;
        #endregion

        #region CONSTRUCTOR
        public MasterDataAccess(ISqlDbHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        #endregion

        #region PUBLIC
        public async Task<IEnumerable<Equipments>> GetEquipments()
        {
            const string Query = @"
                                  SELECT
                                    [Id],
                                    [Name],
                                    [Description]
                                  FROM 
                                    [OTM].[EquipmentsMaster]
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            const string StoredProcedure = "[OTM].[SelectDepartments]";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<Departments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;

        }
        public async Task<IEnumerable<Anaesthesia>> GetAnaesthesiaList()
        {
            const string Query = @"
                                    SELECT 
                                        [Id],[Name],[ModifiedBy]
                                    FROM 
                                        [OTM].[AnaesthesiaTypeMaster]
                                    ORDER BY 
                                        Name
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<Anaesthesia>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        // Employees START
        public async Task<IEnumerable<Employee>> GetEmployees(string departmentArray)
        {
            const string StoredProcedure = "[OTM].[SelectEmployeesWithDepartmentsMapping]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@DepartmentsToFetchFrom", departmentArray);
            var result = await _sqlHelper.QueryAsync<Employee>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEmployeesWithCategoryId(int emplyoeeCategoryId)
        {
            const string Query = @"
                                    SELECT 
                                        Employees.Id                    AS EmployeeId,
                                        Employees.Code                  AS EmployeeCode,
                                        Employees.Title                 AS Title,
                                        Employees.FirstName             AS FirstName,
                                        Employees.LastName              AS LastName,
                                        Employees.MiddleName            AS MiddleName,
                                        Employees.DepartmentID          AS DepartmentId,
                                        [EmployeeCategories].[Id]       AS EmployeeCategoryId,
                                        [EmployeeCategories].[Name]     AS CategoryName,
                                        dbo.Departments.TypeCode        AS departmentTypeCode,
                                        dbo.Departments.Name            AS departmentName
                                    FROM 
                                        HR.Employees
                                    INNER JOIN 
                                        [dbo].[Departments] ON [HR].[Employees].DepartmentID= [dbo].[Departments].[Id]
                                    LEFT JOIN 
                                            [HR].[EmployeeCategories] AS EmployeeCategories ON [HR].[Employees].CategoryID = EmployeeCategories.[Id]
                                    WHERE
                                            HR.Employees.CurrentStatus='A'
                                        AND
                                            HR.Employees.Active=1
                                        AND 
                                            dbo.Departments.TypeCode=5
                                        AND 
                                            EmployeeCategories.Id=@emplyoeeCategoryId
                                    ORDER BY 
                                        HR.Employees.FirstName

                                 ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@emplyoeeCategoryId", emplyoeeCategoryId);
            var result = await _sqlHelper.QueryAsync<Employee>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        
        public async Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode)
        {
            const string StoredProcedure = "[OTM].[SelectEmployeeDetails]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@EmployeeCode", employeeCode);
            var result = await _sqlHelper.QueryAsync<Employee>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        // Employees END


        public async Task<IEnumerable<UserRoleDetails>> GetOTUserRole(int employeeId)
        {
            const string Query = @"
                                    SELECT
                                        [OtmUser].[Id]                   AS OtmUserId           ,
                                        [OtmUser].[userId]               AS EmployeeId          ,
                                        [OtmUser].[UserRoleId]           AS UserRoleId          ,
                                        [UserRoles].[name]               AS RoleName            ,
                                        [dboUsers].[userName]            AS UserName            ,
                                        [Employees].[FirstName]          AS FirstName           ,
                                        [Employees].[LastName]           AS LastName            ,
                                        [Employees].[MiddleName]         AS MiddleName          ,
                                        [Employees].[DepartmentID]       AS DepartmentID        ,
                                        [dbo].[Departments].[TypeCode]   AS DepartmentTypeCode  ,
                                        [dbo].[Departments].[Name]       AS DepartmentName
                                    FROM 
                                        [behive-dev-otm].[OTM].[Users] AS OtmUser 
                                    LEFT JOIN  
                                        [dbo].[Users] AS dboUsers ON OtmUser.userId = dboUsers.EmployeeId
                                    LEFT JOIN 
                                        [HR].[Employees] as Employees ON OtmUser.userId = Employees.Id
                                    LEFT JOIN 
                                        [dbo].[Departments] ON [Employees].[DepartmentID]= [dbo].[Departments].[Id]
                                    LEFT JOIN 
                                        [OTM].[UserRoles] as UserRoles ON OtmUser.userRoleId = UserRoles.Id
                                    WHERE 
                                        [OtmUser].[userId]=@EmployeeId
                                 ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@EmployeeId", employeeId);

            var result = await _sqlHelper.QueryAsync<UserRoleDetails>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<AvailableRoles>> GetOTRoles()
        {
            const string Query = @"
                                    SELECT 
                                         [Id]
                                        ,[name]
                                    FROM 
                                        [behive-dev-otm].[OTM].[UserRoles]
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<AvailableRoles>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<int>> PostNewOTUser(int userId, int roleId)
        {
            const string Query = @"
                                    INSERT INTO [behive-dev-otm].[OTM].[Users]
                                    (
                                        [userId] ,
                                        [UserRoleId]
                                    )
                                    VALUES
                                    (
                                        @userId,
                                        @userRoleId
                                    )
                                 ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@userId", userId);
            SqlParameters.Add("@userRoleId", roleId);
            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<int>> CreateAdminRolesAndRigthts(PostAdminRolesAndRights otAdminAndRights)
        {
            const string Query = @"
                                        INSERT INTO [behive-dev-otm].[OTM].[UserRoles]
                                        (
                                             [name]
                                            ,[Active]
                                            ,[DisplayName]
                                        )
                                        VALUES
                                        (
                                            @UserRoleName,1,@UserRoleName
                                        );
                                        


                                        -- fetch new user id
                                        DECLARE @NewRoleId AS INT;
                                        SET @NewRoleId = SCOPE_IDENTITY();

                                        -- insert role permissions start
                                        INSERT INTO [behive-dev-otm].[OTM].[RoleHasPermissions]
                                        (
                                            [ResourceId],
                                            [AccessType],
                                            [UserRoleId]
                                        )
                                        SELECT 
                                            [ResourceId],
                                            [AccessType],
                                            [UserRoleId]=@NewRoleId
                                        FROM 
                                            OPENJSON(@ResourceAndAccess)
                                            WITH  
                                            (
                                                ResourceId        int     '$.resourceId',  
                                                AccessType        int     '$.PermissionId'
                                            );
                                        -- insert role permissions end

                                        -- add new user role end

                                 ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@UserRoleName", otAdminAndRights.UserRoleName);
            SqlParameters.Add("@ResourceAndAccess", otAdminAndRights.ResourceAndAccess);
            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<UserResources>> GetOTRolePermissions(int? roleId)
        {
            const string Query = @"
                                    SELECT
                                         [Permissions].[Id]           AS PermissionId
                                        ,[Permissions].[UserRoleId]   AS UserRoleId
                                        ,[Permissions].[ResourceId]   AS ResourceId
                                        ,[Resources].[name]           AS ResourceName
                                        ,[Resources].[Description]    AS ResourceDescription
                                        ,[Permissions].[AccessType]   AS AccessType
                                    FROM 
                                        [behive-dev-otm].[OTM].[RoleHasPermissions] as Permissions
                                    LEFT JOIN
                                        [OTM].[Resources] AS Resources ON ResourceId = Resources.Id
                                    WHERE UserRoleId =@roleId
                                 ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@roleId", roleId);

            var result = await _sqlHelper.QueryAsync<UserResources>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Resources>> GetOTResources()
        {
            const string Query = @"
                                    SELECT
                                         [Id]
                                        ,[name]
                                        ,[Description]
                                        ,[Active]
                                    FROM 
                                        [behive-dev-otm].[OTM].[Resources]
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<Resources>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatres()
        {
            const string Query = @"
                                    SELECT 
                                        [Id] AS OperationTheatreId,
                                        [Name]                    ,
                                        [Location]                ,
                                        [Type]                    ,
                                        [DepartmentId]            ,
                                        [CleaningTime]            ,
                                        [ModifiedBy]
                                    FROM [OTM].[OperationTheatreMaster]                     
                                 ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<OperationTheatre>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword = "")
        {
            const string StoredProcedure = "[OTM].[SelectSurgeries]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PageNumber", _pageNumber);
            SqlParameters.Add("@RowsOfPage", _rowsPerPage);
            SqlParameters.Add("@Search", _searchKeyword);

            var result = await _sqlHelper.QueryAsync<Surgery>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<GetAllocation>> GetAllocations(string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[SelectAllocation]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@StartDate", startDate);
            SqlParameters.Add("@EndDate", endDate);
            var result = await _sqlHelper.QueryAsync<GetAllocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }


        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            string Query = @"
                                INSERT INTO [OTM].[OperationTheatreAllocation]
                                ( 
                                    [OperationTheatreId],[AssignedDepartmentId],[GroupId],[StartDate],[EndDate],[ModifiedBy]
                                )
                                VALUES
                                ( 
                                    @OperationTheatreId, @AssignedDepartmentId, @GroupId, @StartDate, @EndDate, @ModifiedBy
                                )
                            ";
            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@OperationTheatreId", _allocation.OperationTheatreId);
            SqlParameters.Add("@AssignedDepartmentId", _allocation.AssignedDepartmentId);
            SqlParameters.Add("@GroupId", _allocation.GroupId);
            SqlParameters.Add("@StartDate", _allocation.StartDate);
            SqlParameters.Add("@EndDate", _allocation.EndDate);
            SqlParameters.Add("@ModifiedBy", _allocation.ModifiedBy);

            var result = await _sqlHelper.QueryAsync<Allocation>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<Allocation>> EditAllocation(Allocation _allocation)
        {
            string Query = @"
                UPDATE [OTM].[OperationTheatreAllocation]
                SET
                    [OperationTheatreId] = @OperationTheatreId,
                    [AssignedDepartmentId] = @AssignedDepartmentId,
                    [GroupId] = @GroupId,
                    [StartDate] = @StartDate,
                    [EndDate] = @EndDate,
                    [ModifiedBy] = @ModifiedBy
                WHERE
                    [id] = @id
                    AND [AssignedDepartmentId] = @allocationAssignedDepartmentId
            ";

            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@OperationTheatreId", _allocation.OperationTheatreId);
            SqlParameters.Add("@AssignedDepartmentId", _allocation.AssignedDepartmentId);
            SqlParameters.Add("@GroupId", _allocation.GroupId);
            SqlParameters.Add("@StartDate", _allocation.StartDate);
            SqlParameters.Add("@EndDate", _allocation.EndDate);
            SqlParameters.Add("@ModifiedBy", _allocation.ModifiedBy);
            SqlParameters.Add("@id", _allocation.id);
            SqlParameters.Add("@allocationAssignedDepartmentId", _allocation.AssignedDepartmentId);

            var result = await _sqlHelper.QueryAsync<Allocation>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<int>> DeleteAllocations(string allocationIds)
        {
            string Query = @"
                                DELETE FROM 
                                    [OTM].[OperationTheatreAllocation]
                                WHERE 
                                    [Id] IN (SELECT value FROM OPENJSON(@IdArray))
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@IdArray", allocationIds);

            var result = await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<DateTime>> GetDateToday()
        {
            const string Query = "SELECT GETDATE()";

            var SqlParameters = new DynamicParameters();

            var result = await _sqlHelper.QueryAsync<DateTime>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        #endregion

        // ##############################
        // QUESTION_HANDLES SECTION START
        // this section handles form questions
        #region QUESTION_HANDLES
        // Insert section START

        public async Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question)
        {
            const string StoredProcedure = "[OTM].[InsertFormQuestion]";

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@otStageId", question.otStageId.ToString());
            SqlParameters.Add("@FormQuestionTypeId", question.FormQuestionTypeId.ToString());
            SqlParameters.Add("@SubQuestionDisplayOptionId", question.SubQuestionDisplayOptionId.ToString());
            SqlParameters.Add("@name", question.name);
            SqlParameters.Add("@question", question.question);
            SqlParameters.Add("@accessibleTo", question.accessibleTo);
            SqlParameters.Add("@Options", question.Options);
            SqlParameters.Add("@IsRequired", question.IsRequired);
            SqlParameters.Add("@IsDisabled", question.IsDisabled);
            SqlParameters.Add("@HasSubQuestion", question.hasSubQuestion);
            SqlParameters.Add("@SubQuestionTypeId", 4);
            SqlParameters.Add("@SubQuestion", question.subQuestion);
            SqlParameters.Add("@SubQuestionOptions", "");
            SqlParameters.Add("@ModifiedBy", question.ModifiedBy);

            var result = await _sqlHelper.QueryAsync<PostQuestionsModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;

        }
        public async Task<IEnumerable<string>> PostQuestionType(string name, string label)
        {
            string Query = @"
                                INSERT INTO [OTM].[FormQuestionType] 
                                (
                                    [Name],
                                    [Label]
                                )
                                VALUES 
                                ( 
                                    @name,
                                    @Label
                                ) 
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@name", name);
            SqlParameters.Add("@label", label);
            var result = await _sqlHelper.QueryAsync<string>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<string>> PostOtStages(string name, string label)
        {
            string Query = @"
                                INSERT INTO [OTM].[OtStages] 
                                (
                                    [Name]
                                    [Label]
                                )
                                VALUES 
                                ( 
                                    @name,
                                    @label
                                ) 
                            ";

            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@name", name);
            SqlParameters.Add("@label", label);

            var result = await _sqlHelper.QueryAsync<string>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        // Insert section END

        // SELECT section START
        public async Task<IEnumerable<GetQuestions>> GetFormQuestions()
        {
            const string StoredProcedure = "[OTM].[SelectFormQuestions]";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<GetQuestions>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        // SELECT section END

        // SELECT section START
        public async Task<IEnumerable<FormSections>> GetFormSections()
        {
            string Query = @"
                                    SELECT 
                                        [Id],
                                        [Name],
                                        [Label]
                                    FROM 
                                    [OTM].[OtStages]
                             ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<FormSections>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<FormSections>> GetFormQuestionType()
        {
            string Query = @"
                                    SELECT 
                                        [Id],
                                        [Name],
                                        [Label]
                                    FROM 
                                        [OTM].[FormQuestionType]
                             ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<FormSections>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        // SELECT section END


        #endregion
        // QUESTION_HANDLES SECTION END

        // ANSWER HANDLE SECTION START
        public async Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer)
        {
            // Id IN (SELECT value FROM OPENJSON(@IdArray))
            string Query = @"

                                -- delete start
                                DELETE FROM [OTM].[FormAnswer]
                                WHERE 
                                    eventId=@eventId
                                    AND 
                                    questionid IN (SELECT value FROM OPENJSON(@questionIdArray))
                                -- delete end
                                

                                INSERT INTO [OTM].[FormAnswer]
                                SELECT * 
                                FROM OPENJSON(@josnAnswers, '$.answers')
                                WITH  (
                                        eventId             int             '$.eventId',  
                                        questionid          int             '$.questionId', 
                                        answer              varchar(1000)   '$.answer',
                                        answerOptionsId     varchar(1000)   '$.answerOptionsId',
                                        postedBy            varchar(1000)   '$.postedBy'
                                    );

                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@eventId", answer.eventId);
            SqlParameters.Add("@josnAnswers", answer.answersJsonString);
            SqlParameters.Add("@questionIdArray", answer.questionIdArray);
            var result = await _sqlHelper.QueryAsync<PostAnswer>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId)
        {
            string Query = @"
                                SELECT
                                    [Id],
                                    [eventId],
                                    [questionid],
                                    [answer],
                                    [answerOptionsId]
                                FROM 
                                    [OTM].[FormAnswer]
                                WHERE 
                                    [eventId]=@eventId
                            ";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@eventId", eventId);
            var result = await _sqlHelper.QueryAsync<GetAnswer>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        // ANSWER HANDLE SECTION END

        public async Task<IEnumerable<GetAllocation>> CheckAllocationByOperationThearter(string startDate, string endDate, int operationTheatreId)
        {
            string Query = @"
                                SELECT 
                                      [OTAllocation].[Id]                        AS Id
                                    , [OTAllocation].[OperationTheatreId]        AS OperationTheatreId
                                    , [OTAllocation].[AssignedDepartmentId]      AS AssignedDepartmentId
                                    , [Departments].[Name]                       AS AssignedDepartmentName
                                    , [OperationTheatre].[Name]                    AS OperationTheatreName
                                    , [OTAllocation].[StartDate]                 AS StartDate
                                    , [OTAllocation].[EndDate]                   AS EndDate
                                    , [OTAllocation].[ModifiedBy]                AS ModifiedBy

                                FROM
                                    [OTM].[OperationTheatreAllocation] AS OTAllocation
                                LEFT JOIN
                                [dbo].[Departments] AS Departments ON OTAllocation.AssignedDepartmentId = Departments.Id
                                LEFT JOIN
                                [OTM].[OperationTheatreMaster] AS OperationTheatre ON OTAllocation.OperationTheatreId = OperationTheatre.Id

                                WHERE 
                                    (([StartDate] <= @StartDateToSearch AND [EndDate] >= @StartDateToSearch)
                                    OR ([StartDate] <= @EndDateToSearch AND [EndDate] >= @EndDateToSearch)
                                    OR ([StartDate] >= @StartDateToSearch AND [EndDate] <= @EndDateToSearch))
                                    AND [OperationTheatreId] = @operationTheatreId;
                            ";
            // const string StoredProcedure = "[OTM].[CheckifAllocationExistsForOperationTheaterInRange]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@operationTheatreId", operationTheatreId);
            SqlParameters.Add("@StartDateToSearch", startDate);
            SqlParameters.Add("@EndDateToSearch", endDate);
            var result = await _sqlHelper.QueryAsync<GetAllocation>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        // Non Op maste
        public async Task<IEnumerable<NonOperativeProcedureList>> GetNonOperativeProceduresList()
        {
            string Query = @"
                                SELECT 
                                    [Id]
                                    ,[Name]
                                    ,[Label]
                                    ,[Description]
                                    ,[IsActive]
                                FROM 
                                    [behive-dev-otm].[OTM].[NonOperativeProceduresListMaster]
                            ";
            var SqlParameters = new DynamicParameters();
            var result = await _sqlHelper.QueryAsync<NonOperativeProcedureList>(Query, SqlParameters, CommandType.Text);
            return result;
        }


    }
}
