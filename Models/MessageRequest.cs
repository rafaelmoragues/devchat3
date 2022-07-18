namespace devchat3.Models
{
    public class MessageRequest
    {
        public string UserId { get; set; }
        public int RoomChatId { get; set; }
        public string MessageBody { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
    }
}
