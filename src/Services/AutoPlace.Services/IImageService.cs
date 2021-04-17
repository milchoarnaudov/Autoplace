namespace AutoPlace.Services
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        public Task<bool> Save(IFormFile file, string path, string imageId);

        public string GetExtension(string fileName);
    }
}
