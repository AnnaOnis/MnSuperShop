
using System.ComponentModel.DataAnnotations;

namespace MyShop.HttpApiCient.Models
{
    public class Account
    {
        [Required]
        public Guid Id { get; init; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string? Password { get; set; }
    }
}
