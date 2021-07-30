namespace AutoPlace.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Messages;
    using AutoPlace.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task<int> CreateAsync(CreateMessage message)
        {
            if (message is null ||
                string.IsNullOrWhiteSpace(message.Topic) ||
                string.IsNullOrWhiteSpace(message.Content) ||
                message.AutopartId == default ||
                message.ReceiverId == default ||
                message.SenderId == default)
            {
                return 0;
            }

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

            return messageEntity.Id;
        }

        public IEnumerable<T> GetAllByUser<T>(string userId) =>
            this.messagesRepository.AllAsNoTracking()
                .Where(x => x.SenderId == userId)
                .To<T>()
                .ToList();

        public IEnumerable<T> GetAllForUser<T>(string userId) =>
            this.messagesRepository.AllAsNoTracking()
                .Where(x => x.ReceiverId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();

        public T GetById<T>(int id) =>
            this.messagesRepository.AllAsNoTracking()
                .Where(x => x.Id == id).To<T>()
                .FirstOrDefault();

        public async Task<bool> DeleteAsync(int id)
        {
            var message = this.messagesRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (message == null)
            {
                return false;
            }

            this.messagesRepository.Delete(message);
            await this.messagesRepository.SaveChangesAsync();

            return true;
        }
    }
}
