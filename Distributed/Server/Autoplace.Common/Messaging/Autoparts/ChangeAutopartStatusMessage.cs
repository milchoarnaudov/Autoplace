using Autoplace.Common.Enums;

namespace Autoplace.Common.Messaging.Autoparts
{
    public class ChangeAutopartStatusMessage
    {
        public string MessageDataId { get; set; }

        public string AutopartId { get; set; }

        public AutopartStatus NewStatus { get; set; }

        public DateTime DateTimeOfApproval { get; set; }
    }
}
