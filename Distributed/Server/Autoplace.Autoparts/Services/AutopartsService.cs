using AutoMapper;
using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Specifications.Autoparts;
using Autoplace.Common;
using Autoplace.Common.Models;
using Autoplace.Common.Services;
using AutoPlace.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Services
{
    public class AutopartsService : BaseDeletableDataService<Autopart>, IAutopartsService
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;
        private readonly IImageService imageService;

        public AutopartsService(
            DbContext dbContext,
            IMapper mapper,
            ILogger logger,
            IImageService imageService)
            : base(dbContext)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.imageService = imageService;
        }

        public async Task<Result<BaseAutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string userId, string imagePath)
        {
            if (createAutopartInputModel == null || String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(imagePath))
            {
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.InvalidArgumentsErrorMessage);
            }

            var autopartEntity = new Autopart
            {
                Name = createAutopartInputModel.Name,
                Price = createAutopartInputModel.Price,
                Description = createAutopartInputModel.Description,
                CategoryId = createAutopartInputModel.CategoryId,
                ConditionId = createAutopartInputModel.ConditionId,
                UserId = userId,
                CarId = createAutopartInputModel.CarId,
            };

            try
            {
                await SaveImagesAsync(createAutopartInputModel.Images, imagePath, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            try
            {
                await Data.AddAsync(autopartEntity);
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return Result<BaseAutopartOutputModel>.Success(outputModel);
        }

        public async Task<Result<AutopartOutputModel>> GetById(int id)
        {
            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return Result<AutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            var outputModel = mapper.Map<AutopartOutputModel>(autopartEntity);

            return Result<AutopartOutputModel>.Success(outputModel);
        }

        public IEnumerable<AutopartOutputModel> GetAll(Expression<Func<Autopart, bool>> filteringPredicate, int itemsPerPage = SystemConstants.DefaultMaxItemsConstraint, int page = 0)
        {
            var autoparts = GetDetailedAutopartRecords()
                .Where(filteringPredicate)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(a => mapper.Map<AutopartOutputModel>(a));

            return autoparts;
        }

        public IEnumerable<AutopartOutputModel> Search(SearchFiltersInputModel searchFilters = null)
        {
            if (searchFilters == null)
            {
                return GetAll(autopart => true);
            }

            var specification = GetSpecification(searchFilters);

            if (searchFilters.Page == null || searchFilters.PageSize == null)
            {
                return GetAll(specification);
            }

            return GetAll(specification, searchFilters.PageSize.Value, searchFilters.Page.Value);
        }

        public async Task<Result<BaseAutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string imagePath)
        {
            if (editAutopartInputModel == null)
            {
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.InvalidArgumentsErrorMessage);
            }

            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == editAutopartInputModel.Id);

            if (autopartEntity == null)
            {
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.Name = editAutopartInputModel.Name;
            autopartEntity.Price = editAutopartInputModel.Price;
            autopartEntity.Description = editAutopartInputModel.Description;

            try
            {
                await SaveImagesAsync(editAutopartInputModel.Images, imagePath, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return Result<BaseAutopartOutputModel>.Success(outputModel);
        }

        public async Task<bool> CheckIfUserIsOwnerAsync(string userId, int autopartId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            var autopart = await GetAllRecords()
                .FirstOrDefaultAsync(x => x.Id == autopartId);

            if (autopart == null || autopart.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<Result<BaseAutopartOutputModel>> DeleteAsync(int id)
        {
            var autopartEntity = GetAllRecords()
               .FirstOrDefault(x => x.Id == id);

            if (autopartEntity == null)
            {
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            RemoveRecord(autopartEntity);

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return Result<BaseAutopartOutputModel>.Success(outputModel);
        }

        public int GetCount() => GetAllRecords().Count();

        public async Task<Result> IncreaseViewsCountAsync(int id)
        {
            var autopartEntity = await GetAllRecords().FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return Result.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.ViewsCount++;

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorDuringOperationErrorMessage);
            }

            return Result.Success();
        }

        public async Task<Result<BaseAutopartOutputModel>> MarkAsApproved(int id)
        {
            var autopartEntity = await GetAllRecords().FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.IsApproved = true;

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return Result<BaseAutopartOutputModel>.Success(outputModel);
        }

        public IEnumerable<AutopartOutputModel> GetAllForApproval(int? itemsPerPage, int? page)
        {
            if (page == null || itemsPerPage == null)
            {
                return GetAll(a => !a.IsApproved);
            }

            return GetAll(a => !a.IsApproved, itemsPerPage.Value, page.Value);
        }

        private async Task SaveImagesAsync(IEnumerable<IFormFile> images, string imagePath, Autopart autopartEntity)
        {
            if (images == null || images.Count() == 0)
            {
                return;
            }

            foreach (var image in images)
            {
                var extension = imageService.GetExtension(image.FileName);
                var imageEntity = new Image
                {
                    Extension = extension
                };

                var operationResult = await imageService.Save(image, imagePath, imageEntity.Id);
                if (!operationResult.IsSuccessful)
                {
                    throw new Exception(string.Join(Environment.NewLine, operationResult.ErrorMessages));
                }

                autopartEntity.Images.Add(imageEntity);
            }
        }

        private Specification<Autopart> GetSpecification(SearchFiltersInputModel searchFilters)
        {
            var specification = new AutopartCarManufacturerSpecification(searchFilters.CarManufacturerId)
                .And(new AutopartCarModelSpecification(searchFilters.CarModelId))
                .And(new AutopartCarTypeSpecification(searchFilters.CarTypeId))
                .And(new AutopartCategorySpecification(searchFilters.CategoryId))
                .And(new AutopartConditionSpecification(searchFilters.ConditionId))
                .And(new AutopartNameSpecification(searchFilters.Name))
                .And(new AutopartPriceSpecification(searchFilters.MaxPrice));

            return specification;
        }

        private IQueryable<Autopart> GetDetailedAutopartRecords()
        {
            var autoparts = GetAllRecords()
                .Include(a => a.Condition)
                .Include(a => a.Category)
                .Include(a => a.Car)
                    .ThenInclude(c => c.Model)
                        .ThenInclude(m => m.Manufacturer)
                .Include(a => a.Car)
                    .ThenInclude(c => c.CarType);

            return autoparts;
        }
    }
}
