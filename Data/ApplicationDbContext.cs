using Microsoft.EntityFrameworkCore;

using EmployeesCrud.Models;
using System;

namespace EmployeesCrud.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        { 


        
        }
       public DbSet<Employee> Employees { get; set; }
    }
}
