using Autoplace.Common.Enums;

namespace Autoplace.Common.Messaging.Autoparts
{
    public class ChangeAutopartStatusMessage
    {
        public string MessageId { get; set; }

        public int AutopartId { get; set; }

        public AutopartStatus NewStatus { get; set; }

        public DateTime DateTimeOfApproval { get; set; }
    }
}
