using ClosedXML.Excel;
using EmployeesCrud.Data;
using EmployeesCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.PortableExecutable;


namespace EmployeesCrud.Controllers
{
   
    public class EmployeeController : Controller
    {
        private readonly string _connectionString;
        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {


                    employees.Add(new Employee
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DOB = (DateTime)reader["DOB"]
                    });

                }

            }

            return View(employees);
        }


        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Employees 
                         (FirstName, LastName, Gender, DOB)
                         VALUES (@FirstName, @LastName, @Gender, @DOB)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            Employee employee = new Employee();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    employee.Id = (int)reader["Id"];
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.LastName = reader["LastName"].ToString();
                    employee.Gender = reader["Gender"].ToString();
                    employee.DOB = (DateTime)reader["DOB"];
                }
            }

            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Employees 
                         SET FirstName=@FirstName,
                             LastName=@LastName,
                             Gender=@Gender,
                             DOB=@DOB
                         WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Employee employee = new Employee();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    employee.Id = (int)reader["Id"];
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.LastName = reader["LastName"].ToString();
                    employee.Gender = reader["Gender"].ToString();
                    employee.DOB = (DateTime)reader["DOB"];
                }
            }

            return View(employee);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int employeeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Employees WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", employeeId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
        public IActionResult ExportToExcel()
        {
            DataTable dt = new DataTable();//in-memorytable bridge between db data and excel
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Gender");
            dt.Columns.Add("DOB");
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "select Id, FirstName, LastName,Gender, DOB from Employees";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dt.Rows.Add(
                        reader["Id"],
                        reader["FirstName"],
                        reader["LastName"],
                        reader["Gender"],
                        Convert.ToDateTime(reader["DOB"]).ToShortDateString()
                        );
                }


            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Employees");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
                }
            }
     



    }
        public IActionResult BulkDelete(string ids)
        {
            var idList = ids.Split(',').Select(int.Parse).ToList();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                foreach (var id in idList)
                {
                    string query = "DELETE FROM Employees WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
