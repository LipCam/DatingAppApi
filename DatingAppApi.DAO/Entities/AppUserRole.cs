using Microsoft.AspNetCore.Identity;

namespace DatingAppApi.DAL.Entities
{
    public class AppUserRole : IdentityUserRole<long>
    {
        public AppUsers User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }
}
