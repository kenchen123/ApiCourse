using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DepartmentController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            {
                var result = conn.QueryMultiple("retrieve_departments", commandType: CommandType.StoredProcedure);
                var dept = result.Read<Department>().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, dept);
            }
        }

        public string Post(Department department)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    conn.Execute("insert_department",
                    new
                    {
                        DepartmentName = department.DepartmentName
                    }, commandType: CommandType.StoredProcedure); ;
                }
                return "Success";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public string Put(Department department)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    conn.Execute("update_department",
                    new
                    {
                        DepartmentID = department.DepartmentID,
                        DepartmentName = department.DepartmentName
                    }, commandType: CommandType.StoredProcedure);
                }
                return "Success";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public string Delete(int id)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    conn.Execute("delete_department",
                    new
                    {
                        DepartmentID = id
                    }, commandType: CommandType.StoredProcedure);
                }
                return "Success";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
