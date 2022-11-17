using Autoplace.Common.Models;

namespace AutoPlace.Services
{
    public interface IImageService
    {
        Task<Result> Save(IFormFile file, string path, string imageId);

        string GetExtension(string fileName);
    }
}
