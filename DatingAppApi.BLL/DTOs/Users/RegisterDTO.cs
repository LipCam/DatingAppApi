using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.BLL.DTOs.Users
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string? KnownAs { get; set; } = string.Empty;

        [Required]
        public string? Gender { get; set; } = string.Empty;

        [Required]
        public string? DateOfBirth { get; set; } = string.Empty;

        [Required]
        public string? City { get; set; } = string.Empty;

        [Required]
        public string? Country { get; set; } = string.Empty;

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;
    }
}
