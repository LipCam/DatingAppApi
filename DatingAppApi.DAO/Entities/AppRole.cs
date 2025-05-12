using Microsoft.AspNetCore.Identity;

namespace DatingAppApi.DAL.Entities
{
    public class AppRole : IdentityRole<long>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
