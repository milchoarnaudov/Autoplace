namespace Autoplace.Common.Messaging.Autoparts
{
    public class ApprovalRequestMessage
    {
        public int AutopartId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public IEnumerable<ImageMessage> Images { get; set; }
    }
}
