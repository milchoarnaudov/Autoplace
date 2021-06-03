namespace AutoPlace.Services
{
    using AutoPlace.Services.Common;

    public interface ITextService : ISingletonService
    {
        string ShortenText(string text, int maxLength);
    }
}
