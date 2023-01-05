using AutoMapper;
using Autoplace.Common.Errors;
using Autoplace.Common.Services;
using Autoplace.Common.Services.Data;
using Autoplace.Members.Common;
using Autoplace.Members.Data.Models;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;
using Microsoft.EntityFrameworkCore;

namespace Autoplace.Members.Services
{
    public class ChatService : BaseDeletableDataService<Chat>, IChatService
    {
        private readonly IMembersService membersService;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ChatService(
            DbContext dbContext,
            IMembersService membersService,
            IMapper mapper,
            ILogger logger)
            : base(dbContext)
        {
            this.membersService = membersService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<OperationResult<ChatOutputModel>> CreateAsync(string receiverUsername, string senderUsername)
        {
            if (string.IsNullOrWhiteSpace(receiverUsername)
                || string.IsNullOrWhiteSpace(senderUsername))
            {
                return OperationResult<ChatOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            if (receiverUsername == senderUsername)
            {
                return OperationResult<ChatOutputModel>.Failure(GenericErrorMessages.OperationNotAllowed);
            }

            var receiverMember = await membersService.GetEntityAsync(m => m.Username == receiverUsername);
            var senderMember = await membersService.GetEntityAsync(m => m.Username == senderUsername);

            if (receiverMember == null || senderMember == null)
            {
                return OperationResult<ChatOutputModel>.Failure(ErrorMessages.MemberNotFound);
            }

            var chatAlreadyExists = await GetAllRecords()
                .Include(m => m.Members)
                .AnyAsync(c => c.Members.Any(m => m.Username == receiverUsername) 
                    && c.Members.Any(m => m.Username == senderUsername));

            if (chatAlreadyExists)
            {
                return OperationResult<ChatOutputModel>.Failure(GenericErrorMessages.OperationNotAllowed);
            }

            var chatEntity = new Chat();
            chatEntity.Members.Add(receiverMember);
            chatEntity.Members.Add(senderMember);
            Data.Add(chatEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<ChatOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                throw;
            }

            var outputModel = mapper.Map<ChatOutputModel>(chatEntity);

            return OperationResult<ChatOutputModel>.Success(outputModel);
        }

        public async Task<IEnumerable<ChatForUserOutputModel>> GetAllForMemberAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Enumerable.Empty<ChatForUserOutputModel>();
            }

            var output = await GetAllRecords()
               .Where(c => c.Members.Any(m => m.Username == username))
               .Select(c => new ChatForUserOutputModel
               {
                   Id = c.Id,
                   LastInteractionDateTime = c.UpdatedOn,
                   WithMember = mapper.Map<MemberOutputModel>(c.Members.FirstOrDefault(m => m.Username != username))
               })
               .ToListAsync();

            return output;
        }

        public async Task<ChatWithMessagesOutputModel> GetAsync(int chatId, string username)
        {
            var chatEntity = GetAllRecords()
                .Include(c => c.ChatMessages)
                .Include(c => c.Members)
                .FirstOrDefault(c => c.Id == chatId);

            if (chatEntity == null)
            {
                return null;
            }

            var member = await membersService.GetEntityAsync(m => m.Username == username);

            if (member == null)
            {
                return null;
            }

            if (!IsAMemberOfTheChat(chatEntity, member.Id))
            {
                return null;
            }

            var outputModel = mapper.Map<ChatWithMessagesOutputModel>(chatEntity);

            return outputModel;
        }


        public async Task<OperationResult<ChatMessageOutputModel>> SendMessageAsync(int chatId, ChatMessageInputModel message, string senderUsername)
        {
            if (message == null || string.IsNullOrWhiteSpace(senderUsername))
            {
                return OperationResult<ChatMessageOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var chatEntity = GetAllRecords()
                .Include(c => c.ChatMessages)
                .Include(c => c.Members)
                .FirstOrDefault(c => c.Id == chatId);

            if (chatEntity == null)
            {
                return OperationResult<ChatMessageOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var member = await membersService.GetAsync(m => m.Username == senderUsername);

            if (!IsAMemberOfTheChat(chatEntity, member.Id))
            {
                return OperationResult<ChatMessageOutputModel>.Failure(GenericErrorMessages.OperationNotAllowed);
            }

            var chatMessageEntity = new ChatMessage
            {
                Content = message.Content,
                SenderId = member.Id,
                ChatId = chatEntity.Id
            };

            chatEntity.UpdatedOn = DateTime.UtcNow;
            chatEntity.ChatMessages.Add(chatMessageEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<ChatMessageOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var outputModel = mapper.Map<ChatMessageOutputModel>(chatMessageEntity);

            return OperationResult<ChatMessageOutputModel>.Success(outputModel);
        }

        private bool IsAMemberOfTheChat(Chat chatEntity, int memberId)
            => chatEntity.Members.Any(c => c.Id == memberId);
    }
}
