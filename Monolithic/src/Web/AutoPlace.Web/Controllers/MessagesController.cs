﻿namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Messages;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Message;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IMessagesService messagesService;
        private readonly IAutopartsService autopartsService;
        private readonly UserManager<ApplicationUser> userManager;

        public MessagesController(
            IMessagesService messagesService,
            IAutopartsService autopartsService,
            UserManager<ApplicationUser> userManager)
        {
            this.messagesService = messagesService;
            this.autopartsService = autopartsService;
            this.userManager = userManager;
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var all = this.messagesService.GetAllForUser<MessageListItemViewModel>(userId);

            return this.View(all);
        }

        public IActionResult Add(int id)
        {
            var autopart = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (autopart == null)
            {
                return this.NotFound();
            }

            if (autopart.OwnerUserName == this.User.FindFirstValue(ClaimTypes.Name))
            {
                return this.Forbid();
            }

            var viewModel = new CreateMessageInputModel
            {
                AutopartName = autopart.Name,
                AutopartId = id,
                ReceiverUsername = autopart.OwnerUserName,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateMessageInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var receiver = await this.userManager.FindByNameAsync(input.ReceiverUsername);
            var message = new CreateMessage
            {
                Topic = input.Topic,
                Content = input.Content,
                AutopartId = input.AutopartId,
                SenderId = currentUserId,
                ReceiverId = receiver.Id,
            };

            await this.messagesService.CreateAsync(message);

            return this.Redirect("/");
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.messagesService.GetById<MessageListItemViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.messagesService.GetById<MessageListItemViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDeletion(int id)
        {
            var currentUserUsername = this.User.Identity.Name;
            var viewModel = this.messagesService.GetById<MessageListItemViewModel>(id);

            if (viewModel.ReceiverUserName != currentUserUsername)
            {
                return this.Forbid();
            }

            var isSuccessful = await this.messagesService.DeleteAsync(id);

            if (!isSuccessful)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }
    }
}
