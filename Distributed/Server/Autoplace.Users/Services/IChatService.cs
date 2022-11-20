using Autoplace.Common.Models;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;

namespace Autoplace.Members.Services
{
    public interface IChatService
    {
        Task<OperationResult<ChatOutputModel>> CreateAsync(string receiverUsername, string senderUsername);

        Task<OperationResult<ChatMessageOutputModel>> SendMessageAsync(ChatMessageInputModel message, string senderUsername);

        Task<ChatWithMessagesOutputModel> GetAsync(int chatId, string username);

        Task<IEnumerable<ChatForUserOutputModel>> GetAllForMemberAsync(string username);
    }
}
