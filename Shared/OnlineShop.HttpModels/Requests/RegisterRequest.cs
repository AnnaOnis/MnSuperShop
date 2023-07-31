using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 


namespace OnlineShop.HttpModels.Requests
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "Имя должно содержать больше 3 символов!", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль не должен быть меньше 8 символов!", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }
    }
}
