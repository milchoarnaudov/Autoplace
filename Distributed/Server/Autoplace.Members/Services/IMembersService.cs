using Autoplace.Common.Services;
using Autoplace.Members.Data.Models;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;
using System.Linq.Expressions;

namespace Autoplace.Members.Services
{
    public interface IMembersService
    {
        Task<OperationResult<MemberOutputModel>> CreateAsync(string userId, string username, string email);

        Task<MemberOutputModel> GetAsync(Expression<Func<Member, bool>> predicate);

        Task<Member> GetEntityAsync(Expression<Func<Member, bool>> predicate);
    }
}
