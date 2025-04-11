namespace DatingAppApi.BLL.Helpers
{
    public class UserLikesParams : PaginationParams
    {
        public long UserId { get; set; }
        public required string Predicate { get; set; } = "liked";
    }
}
