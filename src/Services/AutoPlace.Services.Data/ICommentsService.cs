namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Comments;

    public interface ICommentsService
    {
        Task CreateAsync(CreateCommentDTO comment);
    }
}
