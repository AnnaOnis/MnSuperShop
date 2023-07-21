using System.ComponentModel.DataAnnotations;

namespace MyShop.HttpApiCient.Models
{
    public class RegisterAccountForm
    {
        [Required]
        [StringLength(30, ErrorMessage = "Имя должно содержать больше 3 символов!", MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
    }
}
