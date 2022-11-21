namespace AutoPlace.Web.ViewModels.Message
{
    public class CreateMessageInputModel : BaseMessageViewModel
    {
        public int? AutopartId { get; set; }

        public string AutopartName { get; set; }

        public string ReceiverUsername { get; set; }
    }
}
