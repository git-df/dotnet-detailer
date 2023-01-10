using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVC.Models
{
    public class EmployeeInUserInfoModel
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class AddEmployeeModel
    {
        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Musisz podać email")]
        [EmailAddress(ErrorMessage = "Adres nie jest poprawny")]
        public string Email { get; set; } = string.Empty;
    }
}
