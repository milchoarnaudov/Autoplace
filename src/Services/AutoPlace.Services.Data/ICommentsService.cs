namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.Models.Comments;

    public interface ICommentsService : ITransientService
    {
        Task CreateAsync(CreateComment comment);
    }
}
