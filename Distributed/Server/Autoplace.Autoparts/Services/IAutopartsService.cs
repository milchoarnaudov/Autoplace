using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Common;
using Autoplace.Common.Enums;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Services
{
    public interface IAutopartsService
    {
        Task<OperationResult<BaseAutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string userId, string imagePath);

        Task<IEnumerable<AutopartOutputModel>> SearchAsync(SearchFiltersInputModel searchFilters = null);

        Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1);

        Task<AutopartOutputModel> GetAsync(int id);

        Task<OperationResult<BaseAutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string imagePath);

        Task<OperationResult<BaseAutopartOutputModel>> DeleteAsync(int id);

        Task<int> GetCountAsync();

        Task<OperationResult> IncreaseViewsCountAsync(int id);

        Task<bool> CheckIfUserIsOwnerAsync(string userId, int autopartId);

        Task<OperationResult> ChangeStatus(int autopartId, AutopartStatus newStatus);
    }
}