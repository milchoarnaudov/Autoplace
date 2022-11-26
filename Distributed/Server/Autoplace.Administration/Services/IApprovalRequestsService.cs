using Autoplace.Administration.Data.Models;
using Autoplace.Administration.Models.InputModels;
using Autoplace.Administration.Models.OutputModels;
using Autoplace.Common;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Administration.Services
{
    public interface IApprovalRequestsService
    {
        Task<OperationResult<ApprovalRequestOutputModel>> CreateAsync(
            string autopartId, string name, string description, decimal price, string username, IEnumerable<Image> images);

        Task<IEnumerable<ApprovalRequestOutputModel>> GetAllAsync(Expression<Func<ApprovalRequest, bool>> filterPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1);

        Task<DetailedApprovalRequestOutputModel> GetAsync(int id);

        Task<OperationResult<ApprovalRequestOutputModel>> ChangeStatusAsync(int approvalRequestId, RequestApprovalInputModel requestApprovalInputModel);
    }
}
