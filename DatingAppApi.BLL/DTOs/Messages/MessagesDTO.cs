namespace DatingAppApi.BLL.DTOs.Messages
{
    public class MessagesDTO
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public required string SenderUsername { get; set; }
        public required string SenderPhotoUrl { get; set; }

        public long RecipientId { get; set; }
        public required string RecipientUsername { get; set; }
        public required string RecipientPhotoUrl { get; set; }

        public required string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
