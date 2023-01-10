using Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVC.Models
{
    public class UserLoginModel
    {
        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Musisz podać email")]
        [EmailAddress(ErrorMessage = "Adres nie jest poprawny")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Musisz podać Hasło")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Pozostań zalogowany")]
        public bool KeepLoggedIn { get; set; } = false;
    }

    public class UserInfoModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public EmployeeInUserInfoModel? Employee { get; set; }
    }

    public class UserEmployeeListModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UserRegisterModel
    {
        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Musisz podać email")]
        [EmailAddress(ErrorMessage = "Adres nie jest poprawny")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Imie")]
        [Required(ErrorMessage = "Musisz podać imie")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Musisz podać nazwisko")]
        public string LastName { get; set; } = string.Empty;
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Musisz podać Hasło")]
        [MinLength(8, ErrorMessage = "Minimum 8 znaków")]
        public string Password { get; set; } = string.Empty;
    }

    public class UserEditModel
    {
        public int Id { get; set; }
        [Display(Name = "Imie")]
        [Required(ErrorMessage = "Musisz podać imie")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Musisz podać nazwisko")]
        public string LastName { get; set; } = string.Empty;
    }

    public class UserPasswordChangeModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Musisz podać stare hasło")]
        public string OldPassword { get; set; } = string.Empty;
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Musisz podać nowe hasło")]
        [MinLength(8, ErrorMessage = "Minimum 8 znaków")]
        public string NewPassword { get; set; } = string.Empty;
    }

}
