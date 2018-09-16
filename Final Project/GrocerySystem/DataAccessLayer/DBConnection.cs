using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal static class DBConnection
    {
        public static SqlConnection GetDBConnection()
        {
            var connString = @"Data Source=localhost;Initial Catalog=finalproject;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        }
    }
}
