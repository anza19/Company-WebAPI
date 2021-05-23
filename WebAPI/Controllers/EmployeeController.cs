using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Utilities;
using WebAPI.Models;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private ISqlUtility _sqlUtility;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env, ISqlUtility sqlUtility)
        {
            _configuration = configuration;
            _env = env;
            _sqlUtility = sqlUtility;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string sqlQuery = "SELECT EmployeeId, EmployeeName, Department, convert(varchar(10), DateOfJoining, 120) as DateOfJoining, " +
                "PhotoFileName FROM dbo.Employee";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string sqlQuery = $"INSERT INTO dbo.Employee VALUES('{employee.EmployeeName}', '{employee.Department}', '{employee.DateOfJoining}', '{employee.PhotoFileName}')";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Added new {employee.EmployeeName} to dbo.Employee");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string sqlQuery = $"UPDATE dbo.Employee SET EmployeeName = '{employee.EmployeeName}', Department = '{employee.Department}', DateOfJoining = '{employee.DateOfJoining}', " +
                $"PhotoFileName = '{employee.PhotoFileName}' " +
                $"WHERE EmployeeId = {employee.EmployeeId}";

            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Updated {employee.EmployeeName} in dbo.Employee");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string sqlQuery = $"DELETE FROM dbo.Employee WHERE EmployeeId={id}";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult($"Deleted {id} from dbo.Employee");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                // get the first file name from the request body
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;

                // need to get application path to folder via DI
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    // save file to folder
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            string sqlQuery = $"SELECT Department FROM dbo.Employee";
            string sqlConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
            DataTable table = _sqlUtility.SqlClient(sqlConnectionString, sqlQuery);

            return new JsonResult(table);
        }
    }
}
