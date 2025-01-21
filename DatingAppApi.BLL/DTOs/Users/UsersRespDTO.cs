namespace DatingAppApi.BLL.DTOs.Users
{
    public class UsersRespDTO
    {
        public required string UserName { get; set; }
        public required string KnownAs { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
