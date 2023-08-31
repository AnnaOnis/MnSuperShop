using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModels.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string Password { get; set; }
    }
}