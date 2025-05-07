using System.ComponentModel.DataAnnotations;

namespace DatingAppApi.DAL.Entities
{
    public class Messages
    {
        public long Id { get; set; }

        [MaxLength(50)]
        public required string SenderUsername { get; set; }

        [MaxLength(50)]
        public required string RecipientUsername { get; set; }

        public required string Content { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSent { get; set; } = DateTime.UtcNow;

        public bool SenderDeleted { get; set; }

        public bool RecipientDeleted { get; set; }

        // navigation properties
        public long SenderId { get; set; }
        public AppUsers Sender { get; set; } = null!;

        public long RecipientId { get; set; }
        public AppUsers Recipient { get; set; } = null!;
    }
}
