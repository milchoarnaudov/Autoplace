namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Messages;

    public interface IMessagesService
    {
        public IEnumerable<T> GetAllForUser<T>(string userId);

        public IEnumerable<T> GetAllByUser<T>(string userId);

        public Task CreateAsync(CreateMessageDTO message);

        T GetMessageById<T>(int id);
    }
}
