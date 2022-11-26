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
        Task<OperationResult<AutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string username, string imageDirectory);

        Task<IEnumerable<AutopartOutputModel>> SearchAsync(SearchFiltersInputModel searchFilters = null);

        Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1);

        Task<DetailedAutopartOutputModel> GetAsync(string id);

        Task<OperationResult<AutopartOutputModel>> EditAsync(string id, AutopartInputModel editAutopartInputModel, string imageDirectory);

        Task<OperationResult<AutopartOutputModel>> DeleteAsync(string id);

        Task<int> GetCountAsync();

        Task<OperationResult> IncreaseViewsCountAsync(string id);

        Task<bool> CheckIfUserIsOwnerAsync(string username, string autopartId);

        Task<OperationResult> ChangeStatus(string id, AutopartStatus newStatus);
    }
}