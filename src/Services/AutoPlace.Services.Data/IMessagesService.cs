namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.DTO.Messages;

    public interface IMessagesService : ITransientService
    {
        IEnumerable<T> GetAllForUser<T>(string userId);

        IEnumerable<T> GetAllByUser<T>(string userId);

        Task CreateAsync(CreateMessageDTO message);

        T GetById<T>(int id);

        Task<bool> DeleteAsync(int id);
    }
}
