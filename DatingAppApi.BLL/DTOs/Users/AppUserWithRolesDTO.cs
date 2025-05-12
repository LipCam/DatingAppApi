namespace DatingAppApi.BLL.DTOs.Users
{
    public class AppUserWithRolesDTO
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
