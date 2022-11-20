using Autoplace.Autoparts.Common;
using Autoplace.Common.Errors;
using Autoplace.Common.Models;

namespace AutoPlace.Services
{
    public class FileSystemImageSaverService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public async Task<OperationResult> Save(IFormFile image, string imagePath, string imageId)
        {
            if (image == null || String.IsNullOrWhiteSpace(imagePath) || String.IsNullOrWhiteSpace(imageId))
            {
                return OperationResult.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            Directory.CreateDirectory($"{imagePath}/Autoparts/");

            var extension = this.GetExtension(image.FileName);
            var physicalPath = $"{imagePath}/Autoparts/{imageId}.{extension}";

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                return OperationResult.Failure("Extension of the file is not allowed.");
            }

            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await image.CopyToAsync(fileStream);

            return OperationResult.Success();
        }

        public string GetExtension(string fileName) => Path.GetExtension(fileName).TrimStart('.');
    }
}
