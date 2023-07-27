using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 


namespace OnlineShop.HttpModels.Requests
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
