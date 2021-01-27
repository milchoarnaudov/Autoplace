namespace AutoPlace.Services
{
    using AutoPlace.Common;

    public class TextService : ITextService
    {
        public string ShortenText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength) + "...";
        }
    }
}
