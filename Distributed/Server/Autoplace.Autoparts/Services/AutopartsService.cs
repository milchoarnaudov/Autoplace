using AutoMapper;
using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Specifications.Autoparts;
using Autoplace.Common;
using Autoplace.Common.Errors;
using Autoplace.Common.Models;
using Autoplace.Common.Services.Data;
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

        public async Task<OperationResult<BaseAutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string userId, string imagePath)
        {
            if (createAutopartInputModel == null || String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(imagePath))
            {
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
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
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            try
            {
                await Data.AddAsync(autopartEntity);
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
        }

        public async Task<AutopartOutputModel> GetByIdAsync(int id)
        {
            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return null;
            }

            var outputModel = mapper.Map<AutopartOutputModel>(autopartEntity);

            return outputModel;
        }

        public async Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int itemsPerPage = SystemConstants.DefaultMaxItemsConstraint, int page = 1)
        {
            var autoparts = await GetDetailedAutopartRecords()
                .Where(filteringPredicate)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var output = autoparts.Select(a => mapper.Map<AutopartOutputModel>(a));

            return output;
        }

        public async Task<IEnumerable<AutopartOutputModel>> GetAllForApprovalAsync(int? itemsPerPage, int? page)
        {
            if (page == null || itemsPerPage == null)
            {
                return await GetAllAsync(a => !a.IsApproved);
            }

            return await GetAllAsync(a => !a.IsApproved, itemsPerPage.Value, page.Value);
        }

        public async Task<IEnumerable<AutopartOutputModel>> SearchAsync(SearchFiltersInputModel searchFilters = null)
        {
            if (searchFilters == null)
            {
                return await GetAllAsync(autopart => true);
            }

            var specification = GetSpecification(searchFilters);

            if (searchFilters.Page == null || searchFilters.PageSize == null)
            {
                return await GetAllAsync(specification);
            }

            return await GetAllAsync(specification, searchFilters.PageSize.Value, searchFilters.Page.Value);
        }

        public async Task<OperationResult<BaseAutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string imagePath)
        {
            if (editAutopartInputModel == null)
            {
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == editAutopartInputModel.Id);

            if (autopartEntity == null)
            {
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
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
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
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

        public async Task<OperationResult<BaseAutopartOutputModel>> DeleteAsync(int id)
        {
            var autopartEntity = GetAllRecords()
               .FirstOrDefault(x => x.Id == id);

            if (autopartEntity == null)
            {
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            RemoveRecord(autopartEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
        }

        public int GetCount() => GetAllRecords().Count();

        public async Task<OperationResult> IncreaseViewsCountAsync(int id)
        {
            var autopartEntity = await GetAllRecords().FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return OperationResult.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.ViewsCount++;

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult<BaseAutopartOutputModel>> MarkAsApprovedAsync(int id)
        {
            var autopartEntity = await GetAllRecords().FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.IsApproved = true;

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorDuringOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
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
