namespace AutoPlace.Services
{
    public class TextService : ITextService
    {
        public string ShortenText(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
            {
                return text;
            }

            return $"{text.Substring(0, maxLength)}...";
        }
    }
}
