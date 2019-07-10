using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace WebAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            //var dataTable = new DataTable();
            //var query = @"SELECT EmployeeID, EmployeeName, Department, MailID, CONVERT(varchar(10), DOJ, 120) as DOJ from dbo.Employees";
            //using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            //{
            //    using (var cmd = new SqlCommand(query, conn))
            //    {
            //        using(var da = new SqlDataAdapter(cmd))
            //        {
            //            cmd.CommandType = CommandType.Text;
            //            da.Fill(dataTable);
            //        }
            //    }
            //}
            //return Request.CreateResponse(HttpStatusCode.OK, dataTable);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            {
                var result = conn.QueryMultiple("retrieve_employees", commandType: CommandType.StoredProcedure);
                var emp = result.Read<Employee>().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, emp);
            }
        }

        public string Post(Employee employee)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    conn.Execute("insert_employee",
                    new
                    {
                        EmployeeName = employee.EmployeeName,
                        Department = employee.Department,
                        MailID = employee.MailID,
                        DOJ = employee.DOJ
                    }, commandType: CommandType.StoredProcedure); ;
                }

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Put(Employee employee)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                {
                    conn.Execute("update_employee",
                    new
                    {
                        EmployeeID = employee.EmployeeID,
                        EmployeeName = employee.EmployeeName,
                        Department = employee.Department,
                        MailID = employee.MailID,
                        DOJ = employee.DOJ
                    }, commandType: CommandType.StoredProcedure); ;
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
                    conn.Execute("delete_employee",
                    new
                    {
                        EmployeeID = id
                    }, commandType: CommandType.StoredProcedure);
                }
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

   
}
