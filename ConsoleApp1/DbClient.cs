using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper.Contrib.Extensions;

namespace ConsoleApp1
{
    class DbClient
    {
        private readonly SqlConnection _sqlConnection;

        public DbClient(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public void Insert()
        {
            _sqlConnection.Insert("");

        }
    }
}
