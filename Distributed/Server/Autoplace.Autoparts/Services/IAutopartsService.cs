using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Services
{
    public interface IAutopartsService
    {
        Task<Result<BaseAutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string userId, string imagePath);

        IEnumerable<AutopartOutputModel> Search(SearchFiltersInputModel searchFilters = null);

        IEnumerable<AutopartOutputModel> GetAll(Expression<Func<Autopart, bool>> filteringPredicate, int page, int itemsPerPage);

        IEnumerable<AutopartOutputModel> GetAllForApproval(int? page, int? itemsPerPage);

        Task<Result<AutopartOutputModel>> GetById(int id);

        Task<Result<BaseAutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string imagePath);

        Task<Result<BaseAutopartOutputModel>> DeleteAsync(int id);

        int GetCount();

        Task<Result> IncreaseViewsCountAsync(int id);

        Task<Result<BaseAutopartOutputModel>> MarkAsApproved(int id);

        Task<bool> CheckIfUserIsOwnerAsync(string userId, int autopartId);
    }
}