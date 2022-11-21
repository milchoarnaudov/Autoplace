using AutoMapper;
using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Specifications.Autoparts;
using Autoplace.Common;
using Autoplace.Common.Enums;
using Autoplace.Common.Errors;
using Autoplace.Common.Messaging.Autoparts;
using Autoplace.Common.Models;
using Autoplace.Common.Services.Data;
using Autoplace.Common.Services.Files;
using Autoplace.Common.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Services
{
    public class AutopartsService : BaseDeletableDataService<Autopart>, IAutopartsService
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;
        private readonly IImageService imageService;
        private readonly IMessageService messageService;

        public AutopartsService(
            DbContext dbContext,
            IMapper mapper,
            ILogger logger,
            IImageService imageService,
            IMessageService messageService)
            : base(dbContext)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.imageService = imageService;
            this.messageService = messageService;
        }

        public async Task<OperationResult<BaseAutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string userId, string directory)
        {
            if (createAutopartInputModel == null || String.IsNullOrWhiteSpace(userId) || String.IsNullOrWhiteSpace(directory))
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
                await SaveImagesAsync(createAutopartInputModel.Images, directory, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, ErrorMessages.ErrorWhileSavingImagesErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            await Data.AddAsync(autopartEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var message = new ApprovalRequestMessage
            {
                AutopartId = autopartEntity.Id,
                Name = autopartEntity.Name,
                Description = autopartEntity.Description,
                Price = autopartEntity.Price,
                Images = autopartEntity.Images.Select(i => new ImageMessage
                {
                    Id = i.Id,
                    Extension = i.Extension,
                    RemoteImageUrl = i.RemoteImageUrl,
                })
            };

            try
            {
                await messageService.PublishAsync(message);
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePublishingMessageErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
        }

        public async Task<AutopartOutputModel> GetAsync(int id)
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

        public async Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1)
        {
            var autoparts = await GetDetailedAutopartRecords()
                .Where(filteringPredicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var output = autoparts.Select(a => mapper.Map<AutopartOutputModel>(a));

            return output;
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

        public async Task<OperationResult<BaseAutopartOutputModel>> EditAsync(EditAutopartInputModel editAutopartInputModel, string directory)
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
                await SaveImagesAsync(editAutopartInputModel.Images, directory, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, ErrorMessages.ErrorWhileSavingImagesErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
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
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var outputModel = mapper.Map<BaseAutopartOutputModel>(autopartEntity);

            return OperationResult<BaseAutopartOutputModel>.Success(outputModel);
        }

        public async Task<int> GetCountAsync() => await GetAllRecords().Where(a => a.Status == AutopartStatus.Approved).CountAsync();

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
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<BaseAutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> ChangeStatus(int autopartId, AutopartStatus newStatus)
        {
            var autopartEntity = await GetAllRecords().FirstOrDefaultAsync(a => a.Id == autopartId);

            if (autopartEntity == null)
            {
                return OperationResult.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.Status = newStatus;

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, ErrorMessages.AutopartNotFoundErrorMessage);
                return OperationResult.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            return OperationResult.Success();
        }

        private async Task SaveImagesAsync(IEnumerable<IFormFile> images, string directory, Autopart autopartEntity)
        {
            if (images == null || images.Count() == 0)
            {
                return;
            }

            foreach (var image in images)
            {
                var imageEntity = new Image();

                using var memoryStream = new MemoryStream();
                image.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var operationResult = await imageService.SaveAsync(imageBytes, image.FileName, directory, imageEntity.Id);
                if (!operationResult.IsSuccessful)
                {
                    throw new Exception(string.Join(Environment.NewLine, operationResult.ErrorMessages));
                }

                imageEntity.RemoteImageUrl = operationResult.Model.PhysicalPath;
                imageEntity.Extension = operationResult.Model.Extension;
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
                .Where(a => a.Status == AutopartStatus.Approved)
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
