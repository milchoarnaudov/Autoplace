namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        T GetById<T>(string id);

        T GetByUsername<T>(string username);

        IEnumerable<T> GetAll<T>();

        string GetUserIdByUsername(string username);
    }
}
