using Autoplace.Common.Services;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;

namespace Autoplace.Members.Services
{
    public interface IChatService
    {
        Task<OperationResult<ChatOutputModel>> CreateAsync(string receiverUsername, string senderUsername);

        Task<IEnumerable<ChatForUserOutputModel>> GetAllForMemberAsync(string username);

        Task<ChatWithMessagesOutputModel> GetAsync(int chatId, string username);

        Task<OperationResult<ChatMessageOutputModel>> SendMessageAsync(int chatId, ChatMessageInputModel message, string senderUsername);
    }
}
