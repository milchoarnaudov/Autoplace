namespace AutoPlace.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class ImageService : IImageService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public async Task<bool> Save(IFormFile image, string imagePath, string imageId)
        {
            Directory.CreateDirectory($"{imagePath}/Autoparts/");

            var extension = this.GetExtension(image.FileName);
            var physicalPath = $"{imagePath}/Autoparts/{imageId}.{extension}";

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                // TODO Log error
                return false;
            }

            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await image.CopyToAsync(fileStream);

            return true;
        }

        public string GetExtension(string fileName) => Path.GetExtension(fileName).TrimStart('.');
    }
}
