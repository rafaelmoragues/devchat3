
namespace devchat3.Models
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<MessageResponse>? Messages { get; set; }
    }
}
