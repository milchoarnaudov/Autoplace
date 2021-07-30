namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.Messages;

    public interface IMessagesService
    {
        Task<int> CreateAsync(CreateMessage message);

        IEnumerable<T> GetAllForUser<T>(string userId);

        IEnumerable<T> GetAllByUser<T>(string userId);

        T GetById<T>(int id);

        Task<bool> DeleteAsync(int id);
    }
}
