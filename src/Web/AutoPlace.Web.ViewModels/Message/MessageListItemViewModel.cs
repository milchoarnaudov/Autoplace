namespace AutoPlace.Web.ViewModels.Message
{
    using System;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class MessageListItemViewModel : BaseMessageViewModel, IMapFrom<Message>
    {
        public int Id { get; set; }

        public string SenderUserName { get; set; }

        public string AutopartName { get; set; }

        public int AutopartId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
