namespace AutoPlace.Services
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using Microsoft.AspNetCore.Http;

    public interface IImageService : ITransientService
    {
        Task<bool> Save(IFormFile file, string path, string imageId);

        string GetExtension(string fileName);
    }
}
