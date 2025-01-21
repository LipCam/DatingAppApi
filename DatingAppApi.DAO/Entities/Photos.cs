using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.DAL.Entities
{
    public class Photos
    {
        public long Id { get; set; }

        [MaxLength(150)]
        public required string Url { get; set; }

        public bool IsMain { get; set; }

        [MaxLength(150)]
        public string? PublicId { get; set; }

        //Navigation properties
        public long UserId { get; set; }
        public AppUsers User { get; set; } = null!;
    }
}
