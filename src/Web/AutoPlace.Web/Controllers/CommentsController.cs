namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Comments;
    using AutoPlace.Web.ViewModels.Comments;
    using AutoPlace.Web.ViewModels.Users;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;
        private readonly IUsersService usersService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public CommentsController(
            ICommentsService commentsService,
            IUsersService usersService,
            IHtmlSanitizer htmlSanitizer)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
            this.htmlSanitizer = htmlSanitizer;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCommentInputModel comment)
        {
            var commentatorId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var commentedUserId = this.usersService.GetByUsername<UsersListItemViewModel>(comment.CommentedUserUserName).Id;
            var sanitizedCommentContent = this.htmlSanitizer.Sanitize(comment.Content);

            if (sanitizedCommentContent.Length < 5)
            {
                return this.BadRequest();
            }

            var commentDTO = new CreateCommentDTO
            {
                CommentatorId = commentatorId,
                CommentedUserId = commentedUserId,
                Content = sanitizedCommentContent,
            };

            await this.commentsService.Create(commentDTO);

            return this.Ok();
        }
    }
}
