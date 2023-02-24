using BCMCH.OTM.API.Shared.Master;
using BCMCH.OTM.API.Shared.General;
using BCMCH.OTM.Data.Contract.Master;
using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using System.Data;
using System.Numerics;

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
                                    [BeHiveCoreDev].[OTM].[EquipmentsMaster]
                                 ";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<Equipments>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        
        public async Task<IEnumerable<Departments>> GetDepartments()
        {
            const string StoredProcedure = "[OTM].[SelectDepartments]";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<Departments>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
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
            var result= await _sqlHelper.QueryAsync<Anaesthesia>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEmployees(string searchOption , string departmentArray, int pageNumber, int rowsOfPage )
        {
            const string StoredProcedure = "[OTM].[SelectEmployeesWithDepartmentsMapping]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PageNumber", pageNumber);
            SqlParameters.Add("@RowsOfPage", rowsOfPage);
            SqlParameters.Add("@Search", searchOption);
            SqlParameters.Add("@DepartmentsToFetchFrom", departmentArray);
            var result= await _sqlHelper.QueryAsync<Employee>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<Employee>> GetEmployeeDetails(int employeeCode)
        {
            const string StoredProcedure = "[OTM].[SelectEmployeeDetails]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@EmployeeCode", employeeCode);
            var result= await _sqlHelper.QueryAsync<Employee>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
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
                                    FROM [BeHiveCoreDev].[OTM].[OperationTheatreMaster]                     
                                 ";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<OperationTheatre>(Query, SqlParameters, CommandType.Text);
            return result;
        }

        public async Task<IEnumerable<Surgery>> GetSurgeryList(int _pageNumber, int _rowsPerPage, string? _searchKeyword="")
        {
            const string StoredProcedure = "[OTM].[SelectSurgeries]";
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@PageNumber", _pageNumber);
            SqlParameters.Add("@RowsOfPage", _rowsPerPage );
            SqlParameters.Add("@Search", _searchKeyword );

            var result= await _sqlHelper.QueryAsync<Surgery>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        
        public async Task<IEnumerable<GetAllocationModel>> GetAllocations(string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[SelectAllocation]";
            var SqlParameters = new DynamicParameters();
            // SqlParameters.Add("@DeartmentId"    , departmentId );
            SqlParameters.Add("@StartDate"      , startDate );
            SqlParameters.Add("@EndDate"        , endDate );
            var result= await _sqlHelper.QueryAsync<GetAllocationModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        
        
        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            string Query = @"
                                INSERT INTO [OTM].[OperationTheatreAllocation]
                                ( 
                                    [OperationTheatreId],[AssignedDepartmentId],[StartDate],[EndDate],[ModifiedBy]
                                )
                                VALUES
                                ( 
                                    @OperationTheatreId, @AssignedDepartmentId,@StartDate,@EndDate,@ModifiedBy
                                )
                            ";
            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@OperationTheatreId"     , _allocation.OperationTheatreId );
            SqlParameters.Add("@AssignedDepartmentId"   , _allocation.AssignedDepartmentId );
            SqlParameters.Add("@StartDate"              , _allocation.StartDate );
            SqlParameters.Add("@EndDate"                , _allocation.EndDate );
            SqlParameters.Add("@ModifiedBy"             , _allocation.ModifiedBy );

            var result= await _sqlHelper.QueryAsync<Allocation>(Query, SqlParameters, CommandType.Text);
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
            SqlParameters.Add("@IdArray", allocationIds );
            var result= await _sqlHelper.QueryAsync<int>(Query, SqlParameters, CommandType.Text);
            return result;
        }


        public async Task<IEnumerable<DateTime>> GetDateToday()
        {
            const string Query = "SELECT GETDATE()";
            
            var SqlParameters = new DynamicParameters();

            var result= await _sqlHelper.QueryAsync<DateTime>(Query, SqlParameters, CommandType.Text);
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
            SqlParameters.Add("@otStageId"                      , question.otStageId.ToString() );
            SqlParameters.Add("@FormQuestionTypeId"             , question.FormQuestionTypeId.ToString() );
            SqlParameters.Add("@SubQuestionDisplayOptionId"     , question.SubQuestionDisplayOptionId.ToString() );
            SqlParameters.Add("@name"                           , question.name );
            SqlParameters.Add("@question"                       , question.question );
            SqlParameters.Add("@accessibleTo"                   , question.accessibleTo );
            SqlParameters.Add("@Options"                        , question.Options);
            SqlParameters.Add("@IsRequired"                     , question.IsRequired);
            SqlParameters.Add("@IsDisabled"                     , question.IsDisabled);
            SqlParameters.Add("@HasSubQuestion"                 , question.hasSubQuestion );
            SqlParameters.Add("@SubQuestionTypeId"              , 4 );
            SqlParameters.Add("@SubQuestion"                    , question.subQuestion );
            SqlParameters.Add("@SubQuestionOptions"             , "");
            SqlParameters.Add("@ModifiedBy"                     , question.ModifiedBy);

            var result= await _sqlHelper.QueryAsync<PostQuestionsModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
            
        }
        public async Task<IEnumerable<string>> PostQuestionType(string name,string label)
        {
            string Query =  @"
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
            SqlParameters.Add("@name"    , name  );
            SqlParameters.Add("@label"   , label );
            var result= await _sqlHelper.QueryAsync<string>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<string>> PostOtStages(string name,string label)
        {   
            string Query =  @"
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
            SqlParameters.Add("@name"    , name);
            SqlParameters.Add("@label"   , label);

            var result= await _sqlHelper.QueryAsync<string>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        // Insert section END

        // SELECT section START
        public async Task<IEnumerable<GetQuestions>> GetFormQuestions()
        {
            const string StoredProcedure = "[OTM].[SelectFormQuestions]";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<GetQuestions>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        // SELECT section END

        // SELECT section START
        public async Task<IEnumerable<FormSections>> GetFormSections()
        {
            string Query =  @"
                                    SELECT 
                                        [Id],
                                        [Name],
                                        [Label]
                                    FROM 
                                    [OTM].[OtStages]
                             ";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<FormSections>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<FormSections>> GetFormQuestionType()
        {
            string Query =  @"
                                    SELECT 
                                        [Id],
                                        [Name],
                                        [Label]
                                    FROM 
                                        [OTM].[FormQuestionType]
                             ";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<FormSections>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        // SELECT section END
        

        #endregion
        // QUESTION_HANDLES SECTION END

        // ANSWER HANDLE SECTION START
        public async Task<IEnumerable<PostAnswer>> PostFormAnswer(PostAnswer answer)
        {
            // Id IN (SELECT value FROM OPENJSON(@IdArray))
            string Query =  @"

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
            SqlParameters.Add("@eventId"    , answer.eventId );
            SqlParameters.Add("@josnAnswers"    , answer.answersJsonString );
            SqlParameters.Add("@questionIdArray"    , answer.questionIdArray );
            var result= await _sqlHelper.QueryAsync<PostAnswer>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        public async Task<IEnumerable<GetAnswer>> GetFormAnswer(int eventId)
        {
            string Query =  @"
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
            SqlParameters.Add("@eventId"    , eventId );
            var result= await _sqlHelper.QueryAsync<GetAnswer>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        
        // ANSWER HANDLE SECTION END


        

    }
}
