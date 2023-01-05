using Autoplace.Common.Models.Files;

namespace Autoplace.Common.Services.Files
{
    public interface IImageService
    {
        Task<OperationResult<ImageSavingResult>> SaveAsync(byte[] image, string fileName, string physicalPath, string imageId);
    }
}
