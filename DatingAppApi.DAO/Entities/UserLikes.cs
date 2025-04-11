using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.DAL.Entities
{
    //[PrimaryKey(nameof(SourceUserId), nameof(TargetUserId))]
    public class UserLikes
    {
        public AppUsers SourceUser { get; set; } = null!;
        public long SourceUserId { get; set; }
        public AppUsers TargetUser { get; set; } = null!;
        public long TargetUserId { get; set; }
    }
}
