namespace AutoPlace.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class FileSystemImageService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public async Task<bool> Save(IFormFile image, string imagePath, string imageId)
        {
            Directory.CreateDirectory($"{imagePath}/Autoparts/");

            var extension = this.GetExtension(image.FileName);
            var physicalPath = $"{imagePath}/Autoparts/{imageId}.{extension}";

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                return false;
            }

            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await image.CopyToAsync(fileStream);

            return true;
        }

        public string GetExtension(string fileName) => Path.GetExtension(fileName).TrimStart('.');
    }
}
