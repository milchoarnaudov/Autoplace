using AutoMapper;
using Autoplace.Administration.Data.Models;
using Autoplace.Administration.Models.InputModels;
using Autoplace.Administration.Models.OutputModels;
using Autoplace.Common;
using Autoplace.Common.Enums;
using Autoplace.Common.Errors;
using Autoplace.Common.Messaging.Autoparts;
using Autoplace.Common.Models;
using Autoplace.Common.Services.Data;
using Autoplace.Common.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Autoplace.Administration.Services
{
    public class ApprovalRequestsService : BaseDataService<ApprovalRequest>, IApprovalRequestsService
    {
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly IMessageService messageService;

        public ApprovalRequestsService(
            DbContext dbContext,
            ILogger logger,
            IMapper mapper,
            IMessageService messageService)
            : base(dbContext)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.messageService = messageService;
        }

        public async Task<OperationResult<ApprovalRequestOutputModel>> CreateAsync(int autopartId, string name, string description, decimal price, IEnumerable<Image> images)
        {
            var approvalRequestEntity = new ApprovalRequest
            {
                AutopartId = autopartId,
                Name = name,
                Description = description,
                Price = price,
                Images = images.ToList()
            };

            await Data.AddAsync(approvalRequestEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<ApprovalRequestOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var outputModel = mapper.Map<ApprovalRequestOutputModel>(approvalRequestEntity);

            return OperationResult<ApprovalRequestOutputModel>.Success(outputModel);
        }

        public async Task<OperationResult<ApprovalRequestOutputModel>> ChangeStatusAsync(int approvalRequestId, RequestApprovalInputModel requestApprovalInputModel)
        {
            if (requestApprovalInputModel == null)
            {
                return OperationResult<ApprovalRequestOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var approvalRequestEntity = await GetAllRecords()
                .FirstOrDefaultAsync(ar => ar.Id == approvalRequestId);

            if (approvalRequestEntity == null)
            {
                return OperationResult<ApprovalRequestOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            approvalRequestEntity.Status = requestApprovalInputModel.IsApproved ? AutopartStatus.Approved : AutopartStatus.Rejected;

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<ApprovalRequestOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var messageDate = new ChangeAutopartStatusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                AutopartId = approvalRequestEntity.AutopartId,
                NewStatus = approvalRequestEntity.Status,
                DateTimeOfApproval = DateTime.UtcNow,
            };

            try
            {
                await messageService.PublishAsync(messageDate);
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePublishingMessageErrorMessage);
            }

            var outputModel = mapper.Map<ApprovalRequestOutputModel>(approvalRequestEntity);

            return OperationResult<ApprovalRequestOutputModel>.Success(outputModel);
        }

        public async Task<IEnumerable<ApprovalRequestOutputModel>> GetAllAsync(Expression<Func<ApprovalRequest, bool>> filterPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1)
        {
            var result = await GetAllRecords()
                .Where(filterPredicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var output = result.Select(ar => mapper.Map<ApprovalRequestOutputModel>(ar));

            return output;
        }

        public async Task<DetailedApprovalRequestOutputModel> GetAsync(int id)
        {
            var approvalRequestEntity = await GetAllRecords()
                .FirstOrDefaultAsync(ar => ar.Id == id);

            var outputModel = mapper.Map<DetailedApprovalRequestOutputModel>(approvalRequestEntity);
            outputModel.Status = approvalRequestEntity.Status.ToString();

            return outputModel;
        }
    }
}
