using Dapper;
using KanDo_REST_API.Data.Interface;
using MySql.Data.MySqlClient;
using System.Data;

namespace KanDo_REST_API.Data
{
    public class DataProvider:IDataProvider
    {
        private readonly IDbConnection connection;
        public DataProvider()
        {
            connection = new MySqlConnection(Constants.connectionstring);
        }

        public async Task<IEnumerable<T>> GetAll<T>(string tablename)
        {
            string query = string.Format(Constants.selectallquery, tablename);
            return await connection.QueryAsync<T>(query);
        }

        public async Task<T> GetByID<T>(string tablename, string id)
        {
            string query = string.Format(Constants.selectbyid, tablename, id);
            return await connection.QuerySingleAsync<T>(query, new {id = id});
        }

        public async Task<IEnumerable<T>> GetAllByCondition<T>(string tablename, T entity)
        {
            string condition = "";
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var data = property.GetValue(entity);
                if(data != null)
                {
                    condition += $"{property.Name}=@{property.Name} and ";
                }
            }
            condition = condition.Substring(0, condition.Length - 4);
            string query = string.Format(Constants.selectbycondition, tablename, condition);
            return await connection.QueryAsync<T>(query, entity);
        }

        public async Task<int> Insert<T>(string tablename, T entity)
        {
            var properties = typeof(T).GetProperties();
            var columnNames = properties.Select(p=> p.Name);
            var columns = string.Join(',', columnNames);
            string values = string.Join(',', columnNames.Select(p => $"@{p}"));
            string query = string.Format(Constants.insertquery, tablename, columns, values);
            return await connection.ExecuteAsync(query, entity);
        }

        public async Task<int> Update<T>(string tablename, T entity)
        {
            var properties = typeof(T).GetProperties();
            List<string> sets = new List<string>();
            foreach(var property in properties)
            {
                if(property.Name!="id"&&property.GetValue(entity) != null)
                {
                    sets.Add($"{property.Name}=@{property.Name}");
                }
            }
            if(sets.Count == 0)
            {
                return 0;
            }
            string query = string.Format(Constants.updatequery, tablename, sets);
            return await connection.ExecuteAsync(query, entity);
        }

        public async Task<int> Delete(string tablename, string id)
        {
            string query = string.Format(Constants.deletequery, tablename, id);
            return await connection.ExecuteAsync(query, new {id = id});
        }
    }
}
