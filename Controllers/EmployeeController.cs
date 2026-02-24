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
        private readonly IConfiguration _configuration;
       
        private readonly string _connectionString;
        public EmployeeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
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
                        DateOfBirth = DateOnly.FromDateTime((DateTime)reader["DateOfBirth"])
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
            if (!ModelState.IsValid)
            {
                return View(employee);   // STOP here if validation fails
            }
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Employees 
                         (FirstName, LastName, Gender, DateOfBirth)
                         VALUES (@FirstName, @LastName, @Gender, @DateOfBirth)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);

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
                    employee.DateOfBirth = DateOnly.FromDateTime((DateTime)reader["DateOfBirth"]);
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
                             DateOfBirth=@DateOfBirth
                         WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", employee.Id);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);

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
                    employee.DateOfBirth = DateOnly.FromDateTime((DateTime)reader["DateOfBirth"]);
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
        public IActionResult Search(string searchTerm)
        {
            List<Employee> employees = new List<Employee>();

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += @" WHERE 
                        FirstName LIKE @search OR 
                        LastName LIKE @search OR 
                        YEAR(DateOfBirth) LIKE @search";
                }

                SqlCommand cmd = new SqlCommand(query, con);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                }
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        DateOfBirth = DateOnly.FromDateTime(
    Convert.ToDateTime(reader["DateOfBirth"])
)
                    });
                }

                con.Close();
            }

            return View("Index",employees);
        }
        
    }
}
