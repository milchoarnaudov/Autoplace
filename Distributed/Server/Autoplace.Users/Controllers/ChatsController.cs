using Autoplace.Common.Controllers;
using Autoplace.Common.Errors;
using Autoplace.Common.Services.Identity;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;
using Autoplace.Members.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoplace.Members.Controllers
{
    [Authorize]
    public class ChatsController : BaseApiController
    {
        private readonly IChatService chatService;
        private readonly ICurrentUserService currentUserService;

        public ChatsController(IChatService chatService, ICurrentUserService currentUserService)
        {
            this.chatService = chatService;
            this.currentUserService = currentUserService;
        }

        [HttpPost("{receiverUsername}")]
        public async Task<ActionResult<ChatOutputModel>> Create(string receiverUsername)
        {
            var senderUsername = currentUserService.Username;

            var result = await chatService.CreateAsync(receiverUsername, senderUsername);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatForUserOutputModel>>> GetAllForMember()
        {
            var currentUser = currentUserService.Username;
            var result = await chatService.GetAllForMemberAsync(currentUser);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatWithMessagesOutputModel>> Get(int id)
        {
            var currentUser = currentUserService.Username;

            var chat = await chatService.GetAsync(id, currentUser);

            if (chat == null)
            {
                return BadRequest(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            return Ok(chat);
        }

        [HttpPost("messages")]
        public async Task<ActionResult<ChatMessageOutputModel>> SendMessage(ChatMessageInputModel input)
        {
            var currentUser = currentUserService.Username;
            var result = await chatService.SendMessageAsync(input, currentUser);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }
    }
}
