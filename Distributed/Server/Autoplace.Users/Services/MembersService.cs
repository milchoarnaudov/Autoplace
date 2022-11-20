using AutoMapper;
using Autoplace.Common.Errors;
using Autoplace.Common.Models;
using Autoplace.Common.Services.Data;
using Autoplace.Members.Common;
using Autoplace.Members.Data.Models;
using Autoplace.Members.Models.InputModels;
using Autoplace.Members.Models.OutputModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Autoplace.Members.Services
{
    public class MembersService : BaseDeletableDataService<Member>, IMembersService
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public MembersService(DbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<OperationResult<MemberOutputModel>> CreateAsync(string userId, string username, string email)
        {
            if (string.IsNullOrWhiteSpace(userId)
                || string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(email))
            {
                return OperationResult<MemberOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var memberEntity = new Member
            {
                Email = email,
                UserId = userId,
                Username = username,
            };

            await Data.AddAsync(memberEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<MemberOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<MemberOutputModel>(memberEntity);

            return OperationResult<MemberOutputModel>.Success(outputModel);
        }

        public async Task<MemberOutputModel> GetAsync(Expression<Func<Member, bool>> predicate)
        {
            var memberEntity = await GetAllRecords().FirstOrDefaultAsync(predicate);

            if (memberEntity == null)
            {
                return null;
            }

            var outputModel = mapper.Map<MemberOutputModel>(memberEntity);

            return outputModel;
        }

        public async Task<Member> GetEntity(Expression<Func<Member, bool>> predicate) => await GetAllRecords().FirstOrDefaultAsync(predicate);
    }
}
