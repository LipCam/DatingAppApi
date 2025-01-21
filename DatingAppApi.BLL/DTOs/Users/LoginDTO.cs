using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.BLL.DTOs.Users
{
    public class LoginDTO
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
