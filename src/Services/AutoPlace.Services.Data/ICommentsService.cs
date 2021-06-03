namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.DTO.Comments;

    public interface ICommentsService : ITransientService
    {
        Task CreateAsync(CreateCommentDTO comment);
    }
}
