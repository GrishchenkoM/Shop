using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль подтвержден неверно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Введите адрес почтового ящика")]
        [RegularExpression(@"^([a-zA-Z0-9_\.-]+)@([a-zA-Z0-9_\.-]+)\.([A-Za-z\.]{2,6})$", ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите адрес почтового ящика")]
        [RegularExpression(@"^([a-zA-Z0-9_\.-]+)@([a-zA-Z0-9_\.-]+)\.([A-Za-z\.]{2,6})$", ErrorMessage = "Неверный формат Email")]
        [Compare("Email", ErrorMessage = "Email подтвержден неверно")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        public string Address { get; set; }
        public string Sex { get; set; }
        [RegularExpression(@"([0-9-+]{0,25})", ErrorMessage = "Неверный формат")]
        public string Phone { get; set; }
    }
}