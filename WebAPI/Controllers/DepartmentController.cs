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
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        // In order to access configuration from appsettings.json
        // We need to use depenedency injection
        private readonly IConfiguration _configuration;
        private ISqlUtility _sqlUtility;

        public DepartmentController(IConfiguration configuration, ISqlUtility sqlUtility)
        {
            _configuration = configuration;
            _sqlUtility = sqlUtility;
        }

        // API to GET all records in Department table
        [HttpGet]
        public JsonResult Get()
        {
            string sqlQuery = "SELECT DepartmentId, DepartmentName FROM Department";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult(table);
        }

        // API to POST a new record
        [HttpPost]
        public JsonResult Post(Department department)
        {
            string sqlQuery = $"INSERT INTO dbo.Department VALUES('{department.DepartmentName}')";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Added new {department.DepartmentName} to dbo.Department");
        }

        // API to PUT (update an existing record) data into an existing record 
        [HttpPut]
        public JsonResult Put(Department department)
        {
            string sqlQuery = $"UPDATE dbo.Department SET DepartmentName='{department.DepartmentName}' WHERE DepartmentId={department.DepartmentId}";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Updated {department.DepartmentName} in dbo.Department");
        }

        // API to PUT (update an existing record) data into an existing record 
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string sqlQuery = $"DELETE FROM dbo.Department WHERE DepartmentId={id}";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Deleted record with id:{id} in dbo.Department");
        }

    }
}
