namespace AutoPlace.Services
{
    public class TextService : ITextService
    {
        public string ShortenText(string text, int maxLength = 35)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            if (text.Length <= maxLength)
            {
                return text;
            }

            return text.Substring(0, maxLength) + "...";
        }
    }
}
