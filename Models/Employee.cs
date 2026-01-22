using System.ComponentModel.DataAnnotations;
namespace EmployeesCrud.Models;

public class Employee
    {
    [Key]
        public int Id { get; set; }
    [Required]
        public string FirstName { get; set; }
    [Required]
        public string LastName { get; set; }
    [Required(ErrorMessage="Date of birth is necessary")]
        public DateTime DOB { get; set; }
    [Required]
        public string Gender { get; set; }
    }

