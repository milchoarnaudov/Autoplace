using Autoplace.Common.Errors;
using Autoplace.Common.Models;
using Autoplace.Common.Models.Files;

namespace Autoplace.Common.Services.Files
{
    public class FileSystemImageSaverService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        /// <summary>
        /// Saves image to the file system and returns its path.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <param name="imageId"></param>
        /// <returns>Path to the image.</returns>
        public async Task<OperationResult<ImageSavingResult>> SaveAsync(byte[] image, string fileName, string directory, string imageId)
        {
            if (image == null 
                || image.Length == 0
                || string.IsNullOrWhiteSpace(fileName) 
                || string.IsNullOrWhiteSpace(directory)
                || string.IsNullOrWhiteSpace(imageId))
            {
                return OperationResult<ImageSavingResult>.Failure(GenericErrorMessages.InvalidArgumentsErrorMessage);
            }

            var extension = GetExtension(fileName);

            Directory.CreateDirectory(directory);

            if (!allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                return OperationResult<ImageSavingResult>.Failure(GenericErrorMessages.InvalidFileTypeErrorMessage);
            }

            var physicalPath = $"{directory}/{imageId}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await fileStream.WriteAsync(image);

            var result = new ImageSavingResult
            {
                Extension = extension,
                PhysicalPath = physicalPath,
            };

            return OperationResult<ImageSavingResult>.Success(result);
        }

        private string GetExtension(string fileName) => Path.GetExtension(fileName).TrimStart('.');
    }
}
