using AutoMapper;
using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.Data.Models;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Specifications.Autoparts;
using Autoplace.Common;
using Autoplace.Common.Enums;
using Autoplace.Common.Errors;
using Autoplace.Common.Messaging;
using Autoplace.Common.Messaging.Autoparts;
using Autoplace.Common.Models;
using Autoplace.Common.Services;
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

        public async Task<OperationResult<AutopartOutputModel>> CreateAsync(CreateAutopartInputModel createAutopartInputModel, string username, string imageDirectory)
        {
            if (createAutopartInputModel == null || String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(imageDirectory))
            {
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var autopartEntity = new Autopart
            {
                Name = createAutopartInputModel.Name,
                Price = createAutopartInputModel.Price,
                Description = createAutopartInputModel.Description,
                CategoryId = createAutopartInputModel.CategoryId,
                ConditionId = createAutopartInputModel.ConditionId,
                Username = username,
                CarId = createAutopartInputModel.CarId,
            };

            try
            {
                await SaveImagesAsync(createAutopartInputModel.Images, imageDirectory, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, ErrorMessages.ErrorWhileSavingImagesErrorMessage);
                return OperationResult<AutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            await Data.AddAsync(autopartEntity);

            var messageData = new ApprovalRequestMessage
            {
                MessageDataId = Guid.NewGuid().ToString(),
                AutopartId = autopartEntity.Id,
                Name = autopartEntity.Name,
                Description = autopartEntity.Description,
                Username = autopartEntity.Username,
                Price = autopartEntity.Price,
                Images = autopartEntity.Images.Select(i => new ImageMessage
                {
                    Id = i.Id,
                    Extension = i.Extension,
                    RemoteImageUrl = i.RemoteImageUrl,
                })
            };
            var message = new Message(messageData);

            await messageService.AddMessageAsync(message);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            try
            {
                await messageService.PublishAsync(message);
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePublishingMessageErrorMessage);
            }

            var outputModel = mapper.Map<AutopartOutputModel>(autopartEntity);

            return OperationResult<AutopartOutputModel>.Success(outputModel);
        }

        public async Task<DetailedAutopartOutputModel> GetAsync(string id)
        {
            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return null;
            }

            var outputModel = mapper.Map<DetailedAutopartOutputModel>(autopartEntity);

            return outputModel;
        }

        /// <summary>
        /// Get all approved autoparts.
        /// </summary>
        /// <param name="filteringPredicate"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AutopartOutputModel>> GetAllAsync(Expression<Func<Autopart, bool>> filteringPredicate, int pageSize = SystemConstants.DefaultMaxItemsConstraint, int page = 1)
        {
            var output = await GetAllRecords()
                .Where(a => a.Status == AutopartStatus.Approved)
                .Where(filteringPredicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AutopartOutputModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Price = a.Price,
                    Username = a.Username,
                })
                .ToListAsync();

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

        public async Task<OperationResult<AutopartOutputModel>> EditAsync(string id, AutopartInputModel editAutopartInputModel, string imageDirectory)
        {
            if (editAutopartInputModel == null)
            {
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var autopartEntity = await GetDetailedAutopartRecords()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (autopartEntity == null)
            {
                return OperationResult<AutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            autopartEntity.Name = editAutopartInputModel.Name;
            autopartEntity.Price = editAutopartInputModel.Price;
            autopartEntity.Description = editAutopartInputModel.Description;
            autopartEntity.Status = AutopartStatus.WaitingForApproval;

            try
            {
                await SaveImagesAsync(editAutopartInputModel.Images, imageDirectory, autopartEntity);
            }
            catch (Exception e)
            {
                logger.LogError(e, ErrorMessages.ErrorWhileSavingImagesErrorMessage);
                return OperationResult<AutopartOutputModel>.Failure(ErrorMessages.ErrorWhileSavingImagesErrorMessage);
            }

            var messageData = new ApprovalRequestMessage
            {
                MessageDataId = Guid.NewGuid().ToString(),
                AutopartId = autopartEntity.Id,
                Name = autopartEntity.Name,
                Description = autopartEntity.Description,
                Username = autopartEntity.Username,
                Price = autopartEntity.Price,
                Images = autopartEntity.Images.Select(i => new ImageMessage
                {
                    Id = i.Id,
                    Extension = i.Extension,
                    RemoteImageUrl = i.RemoteImageUrl,
                })
            };
            var message = new Message(messageData);

            await messageService.AddMessageAsync(message);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            try
            {
                await messageService.PublishAsync(message);
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePublishingMessageErrorMessage);
            }

            var outputModel = mapper.Map<AutopartOutputModel>(autopartEntity);

            return OperationResult<AutopartOutputModel>.Success(outputModel);
        }

        public async Task<bool> CheckIfUserIsOwnerAsync(string username, string autopartId)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var autopart = await GetAllRecords()
                .FirstOrDefaultAsync(x => x.Id == autopartId);

            if (autopart == null || autopart.Username != username)
            {
                return false;
            }

            return true;
        }

        public async Task<OperationResult<AutopartOutputModel>> DeleteAsync(string id)
        {
            var autopartEntity = GetAllRecords()
               .FirstOrDefault(x => x.Id == id);

            if (autopartEntity == null)
            {
                return OperationResult<AutopartOutputModel>.Failure(ErrorMessages.AutopartNotFoundErrorMessage);
            }

            RemoveRecord(autopartEntity);

            try
            {
                await Data.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            var outputModel = mapper.Map<AutopartOutputModel>(autopartEntity);

            return OperationResult<AutopartOutputModel>.Success(outputModel);
        }

        public async Task<int> GetCountAsync() => await GetAllRecords().Where(a => a.Status == AutopartStatus.Approved).CountAsync();

        public async Task<OperationResult> IncreaseViewsCountAsync(string id)
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
                return OperationResult<AutopartOutputModel>.Failure(GenericErrorMessages.ErrorWhilePerformingOperationErrorMessage);
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> ChangeStatus(string id, AutopartStatus newStatus)
        {
            var autopartEntity = await GetAllRecords()
                .FirstOrDefaultAsync(a => a.Id == id);

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
