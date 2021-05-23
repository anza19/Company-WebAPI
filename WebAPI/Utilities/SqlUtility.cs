using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Utilities
{
    public interface ISqlUtility
    {
        DataTable SqlClient(string sqlConnectionString, string sqlQuery);
    }

    public class SqlUtility : ISqlUtility
    {

        public DataTable SqlClient(string sqlConnectionString, string sqlQuery)
        {
            DataTable table = new DataTable();

            SqlDataReader dataReader;

            // connect to EmployeeDB
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();
                // execute the command against the table
                using (var sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);

                    // close reader and connection
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return table;
        }

    }
}
