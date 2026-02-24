using System.ComponentModel.DataAnnotations;
namespace EmployeesCrud.Models;

public class Employee
    {
    [Key]
        public int Id { get; set; }
    [Required(ErrorMessage= "First Name is required")]
    
        public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
    [Required(ErrorMessage="Date of birth is necessary")]
        public DateOnly DateOfBirth { get; set; }
    [Required]
        public string Gender { get; set; }
    }

