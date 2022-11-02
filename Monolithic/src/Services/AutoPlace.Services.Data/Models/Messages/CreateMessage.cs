namespace AutoPlace.Services.Data.Models.Messages
{
    public class CreateMessage
    {
        public string Topic { get; set; }

        public string Content { get; set; }

        public int? AutopartId { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }
    }
}
