namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Comments;
    using AutoPlace.Web.ViewModels.Comments;
    using AutoPlace.Web.ViewModels.Users;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;
        private readonly IHtmlSanitizer htmlSanitizer;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(
            ICommentsService commentsService,
            IHtmlSanitizer htmlSanitizer,
            UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.htmlSanitizer = htmlSanitizer;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateCommentInputModel input)
        {
            var commentatorId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var commentedUser = await this.userManager.FindByNameAsync(input.CommentedUserUserName);
            var sanitizedCommentContent = this.htmlSanitizer.Sanitize(input.Content);

            if (sanitizedCommentContent.Length < 5)
            {
                return this.BadRequest();
            }

            var comment = new CreateComment
            {
                CommentatorId = commentatorId,
                CommentedUserId = commentedUser.Id,
                Content = sanitizedCommentContent,
            };

            await this.commentsService.CreateAsync(comment);

            return this.Ok();
        }
    }
}
