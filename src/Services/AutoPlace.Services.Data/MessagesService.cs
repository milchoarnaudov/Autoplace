namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Messages;
    using AutoPlace.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task Create(CreateMessageDTO message)
        {
            var messageEntity = new Message
            {
                Topic = message.Topic,
                Content = message.Content,
                AutopartId = message.AutopartId,
                ReceiverId = message.ReceiverId,
                SenderId = message.SenderId,
            };

            await this.messagesRepository.AddAsync(messageEntity);
            await this.messagesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByUser<T>(string userId)
        {
            return this.messagesRepository.AllAsNoTracking().Where(x => x.SenderId == userId).To<T>();
        }

        public IEnumerable<T> GetAllForUser<T>(string userId)
        {
            return this.messagesRepository.AllAsNoTracking().Where(x => x.ReceiverId == userId).To<T>();
        }

        public T GetMessageById<T>(int id)
        {
            return this.messagesRepository.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }
    }
}
