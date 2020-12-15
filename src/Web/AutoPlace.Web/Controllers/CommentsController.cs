namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Comments;
    using AutoPlace.Web.ViewModels.Comments;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;
        private readonly IUsersService usersService;

        public CommentsController(
            ICommentsService commentsService,
            IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCommentInputModel comment)
        {
            var commentatorId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var commentedUserId = this.usersService.GetByUsername<UsersListItemViewModel>(comment.CommentedUserUserName).Id;

            var commentDTO = new CreateCommentDTO
            {
                CommentatorId = commentatorId,
                CommentedUserId = commentedUserId,
                Content = comment.Content,
            };

            await this.commentsService.Create(commentDTO);

            return this.Ok();
        }
    }
}
