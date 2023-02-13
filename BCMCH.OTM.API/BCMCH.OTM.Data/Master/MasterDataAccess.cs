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
            const string Query = "SELECT"+
                                 " [Id]"+
                                 ",[Name]"+
                                 ",[Description]"+
                                 "FROM [BeHiveCoreDev].[OTM].[EquipmentsMaster]";
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
            const string StoredProcedure = "[OTM].[SelectAnaesthesiaList]";
            var SqlParameters = new DynamicParameters();
            var result= await _sqlHelper.QueryAsync<Anaesthesia>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
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

        public async Task<IEnumerable<OperationTheatreAllocation>> GetOperationTheatreAllocations(int _departmentId=0, string? _fromDate="")
        {
            // if department id is 0 fetches all department datas
            // else fetches with department id
            const string StoredProcedure = "[OTM].[SelectOperationTheatreAllocation]";
            var SqlParameters = new DynamicParameters();
            
            SqlParameters.Add("@DepartmentId", _departmentId);
            SqlParameters.Add("@FromDate", _fromDate );

            var result= await _sqlHelper.QueryAsync<OperationTheatreAllocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<OperationTheatre>> GetOperationTheatres()
        {
            const string Query = "SELECT [Id] AS OperationTheatreId"+
                                 ",[Name]"+
                                 ",[Location]"+
                                 ",[Type]"+
                                 ",[DepartmentId]"+
                                 ",[CleaningTime]"+
                                 ",[ModifiedBy]"+
                                 "FROM [BeHiveCoreDev].[OTM].[OperationTheatreMaster]";
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
        
        public async Task<IEnumerable<GetAllocationModel>> GetAllocation(int departmentId, string startDate, string endDate)
        {
            const string StoredProcedure = "[OTM].[SelectAllocation]";          
            var SqlParameters = new DynamicParameters();
            SqlParameters.Add("@DeartmentId"    , departmentId );
            SqlParameters.Add("@StartDate"      , startDate );
            SqlParameters.Add("@EndDate"        , endDate );
            var result= await _sqlHelper.QueryAsync<GetAllocationModel>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
            return result;
        }
        
        
        public async Task<IEnumerable<Allocation>> PostAllocation(Allocation _allocation)
        {
            const string StoredProcedure = "[OTM].[InsertAllocation]";
            var SqlParameters = new DynamicParameters();

            SqlParameters.Add("@OperationTheatreId"     , _allocation.OperationTheatreId );
            SqlParameters.Add("@AssignedDepartmentId"   , _allocation.AssignedDepartmentId );
            SqlParameters.Add("@StartDate"              , _allocation.StartDate );
            SqlParameters.Add("@EndDate"                , _allocation.EndDate );
            SqlParameters.Add("@ModifiedBy"             , _allocation.ModifiedBy );

            var result= await _sqlHelper.QueryAsync<Allocation>(StoredProcedure, SqlParameters, CommandType.StoredProcedure);
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
        public async Task<IEnumerable<PostQuestionsModel>> PostQuestion(PostQuestionsModel question)
        {
            string Query =  "INSERT INTO [OTM].[FormQuestions]("+
                                    "[FormsectionId],"+
                                    "[FormQuestionType],"+
                                    "[order],"+
                                    "[name],"+
                                    "[question],"+
                                    "[parentId],"+
                                    "[rolesToShow],"+
                                    "[questionTypeId],"+
                                    "[Options] )"+
                                  " VALUES "+
                                  " ( "+
                                        question.FormsectionId.ToString()+
                                    ","+question.FormQuestionType.ToString()+
                                    ","+question.order.ToString()+
                                    ",'"+question.name+"'"+
                                    ",'"+question.question+"'"+
                                    ","+question.parentId.ToString()+
                                    ",'"+question.rolesToShow+"'"+
                                    ","+question.questionTypeId.ToString()+
                                    ",'"+question.Options+"'"+
                                  " ) ";
            Console.WriteLine(Query);

            var SqlParameters = new DynamicParameters();

            var result= await _sqlHelper.QueryAsync<PostQuestionsModel>(Query, SqlParameters, CommandType.Text);
            return result;
        }
        #endregion
        // QUESTION_HANDLES SECTION END

        
    }
}
