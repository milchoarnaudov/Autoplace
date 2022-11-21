namespace AutoPlace.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        Task<bool> Save(IFormFile file, string path, string imageId);

        string GetExtension(string fileName);
    }
}
