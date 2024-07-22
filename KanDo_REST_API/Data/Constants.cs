using System.Configuration;

namespace KanDo_REST_API.Data
{
    public class Constants
    {
        public const string connectionstring = "server=localhost; database=kandodb; uid=root; pwd=1234567890";
        public const string insertquery = "insert into {0} ({1}) values ({2})";
        public const string updatequery = "update {0} set {1} where id=@id";
        public const string deletequery = "delete from {0} where id=@id";
        public const string selectallquery = "select * from {0}";
        public const string selectbyid = "select * from {0} where id=@id";
        public const string selectbycondition = "select * from {0} where {1}";

        public static string GenerateId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public enum Tables
        {
            users,
            boards,
            usertable,
            tasks,
            usertask,
            taskstatus,
            request
        }
    }
}
