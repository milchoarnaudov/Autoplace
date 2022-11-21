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
        Task<OperationResult<AutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string username, string imagePath);

        Task<IEnumerable<AutopartOutputModel>> SearchAsync(SearchFiltersInputModel searchFilters = null);

        Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1);

        Task<DetailedAutopartOutputModel> GetAsync(int id);

        Task<OperationResult<AutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string imagePath);

        Task<OperationResult<AutopartOutputModel>> DeleteAsync(int id);

        Task<int> GetCountAsync();

        Task<OperationResult> IncreaseViewsCountAsync(int id);

        Task<bool> CheckIfUserIsOwnerAsync(string username, int autopartId);

        Task<OperationResult> ChangeStatus(int autopartId, AutopartStatus newStatus);
    }
}