namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Common;

    public interface IUsersService : ITransientService
    {
        T GetById<T>(string id);

        T GetByUsername<T>(string username);

        IEnumerable<T> GetAll<T>();

        string GetUserIdByUsername(string username);
    }
}
