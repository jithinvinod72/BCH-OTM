using Dapper;
using System.Data;

namespace BCMCH.OTM.Infrastucture.AppSettings.Abstracts
{
    public interface ISqlDbHelper
    {
        string ConnectionString { get; set; }
        int Execute(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<int> ExecuteAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        DataTable GetDataTable(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<DataTable> GetDataTableAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        DataSet GetDataSet(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<DataSet> GetDataSetAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        IEnumerable<T> Query<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        IEnumerable<T> QueryFirst<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> QueryFirstAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        T ExecuteScalar<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);
        Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text);


    }
}
