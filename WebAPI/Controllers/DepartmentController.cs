using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        // In order to access configuration from appsettings.json
        // We need to use depenedency injection
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region database connection and query executor method
        private DataTable SqlClient(string sqlQuery)
        {
            DataTable table = new DataTable();
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");

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
        #endregion
        // API to GET all records in Department table
        [HttpGet]
        public JsonResult Get()
        {
            string sqlQuery = "SELECT DepartmentId, DepartmentName FROM Department";

            DataTable table = SqlClient(sqlQuery);

            return new JsonResult(table);
        }

        // API to POST a new record
        [HttpPost]
        public JsonResult Post(Department department)
        {
            string sqlQuery = $"INSERT INTO dbo.Department VALUES('{department.DepartmentName}')";

            DataTable table = SqlClient(sqlQuery);

            return new JsonResult($"Added new {department.DepartmentName} to dbo.Department");
        }

        // API to PUT (update an existing record) data into an existing record 
        [HttpPut]
        public JsonResult Put(Department department)
        {
            string sqlQuery = $"UPDATE dbo.Department SET DepartmentName='{department.DepartmentName}' WHERE DepartmentId={department.DepartmentId}";

            DataTable table = SqlClient(sqlQuery);

            return new JsonResult($"Updated {department.DepartmentName} in dbo.Department");
        }

        // API to PUT (update an existing record) data into an existing record 
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string sqlQuery = $"DELETE FROM dbo.Department WHERE DepartmentId={id}";

            DataTable table = SqlClient(sqlQuery);

            return new JsonResult($"Deleted record with id:{id} in dbo.Department");
        }

    }
}
