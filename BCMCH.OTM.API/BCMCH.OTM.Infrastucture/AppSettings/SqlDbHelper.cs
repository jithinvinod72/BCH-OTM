using BCMCH.OTM.Infrastucture.AppSettings.Abstracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BCMCH.OTM.Infrastucture.AppSettings
{
    public class SqlDbHelper : ISqlDbHelper
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; set; }
        public SqlDbHelper(IConfiguration configuration, IConnectionStrings connectionStrings)
        {
            ConnectionString = configuration["conn"].ToString();
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public DataSet ConvertDataReaderToDataSet(IDataReader data)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!data.IsClosed)
            {
                ds.Tables.Add("Table" + (i + 1));
                ds.EnforceConstraints = false;
                ds.Tables[i].Load(data);
                i++;
            }
            return ds;
        }
        public int Execute(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            int result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = _connection.Execute(sql, dp, transaction, commandType: commandType);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<int> ExecuteAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            int result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = await _connection.ExecuteAsync(sql, dp, transaction, commandType: commandType);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public DataTable GetDataTable(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            DataTable result = new DataTable();
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    var reader = _connection.ExecuteReader(sql, dp, commandType: commandType);
                    result.Load(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<DataTable> GetDataTableAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            DataTable result = new DataTable();
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    var reader = await _connection.ExecuteReaderAsync(sql, dp, commandType: commandType);
                    result.Load(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public DataSet GetDataSet(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            DataSet result = new DataSet();
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    var reader = _connection.ExecuteReader(sql, dp, commandType: commandType);
                    result = ConvertDataReaderToDataSet(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<DataSet> GetDataSetAsync(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            DataSet result = new DataSet();
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    var reader = await _connection.ExecuteReaderAsync(sql, dp, commandType: commandType);
                    result = ConvertDataReaderToDataSet(reader);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public IEnumerable<T> Query<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = _connection.Query<T>(sql, dp, commandType: commandType, transaction: transaction);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result = null;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = await _connection.QueryAsync<T>(sql, dp, commandType: commandType, transaction: transaction);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public IEnumerable<T> QueryFirst<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = _connection.QueryFirst(sql, dp, commandType: commandType, transaction: transaction);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<IEnumerable<T>> QueryFirstAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = await _connection.QueryFirstAsync(sql, dp, commandType: commandType, transaction: transaction);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public T ExecuteScalar<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            T result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = _connection.ExecuteScalar<T>(sql, dp, transaction, commandType: commandType);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            T result;
            try
            {
                using (var _connection = CreateConnection())
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    using (var transaction = _connection.BeginTransaction())
                    {
                        try
                        {
                            result = await _connection.ExecuteScalarAsync<T>(sql, dp, transaction, commandType: commandType);
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

    }
}
