namespace AutoPlace.Services.Tests
{
    using System;
    using System.Linq;
    using Xunit;

    public class TextServiceTests
    {
        [Fact]
        public void ShortenTextShouldWorkCorrectlyWithDefaultMaxLength()
        {
            var textService = new TextService();


            var text = new String(Enumerable.Repeat('D', 55).ToArray());
            var shortenText = textService.ShortenText(text);
            var expectedResult = new string(Enumerable.Repeat('D', 35).ToArray()) + "...";

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldWorkCorrectlyWithGivenMaxLength()
        {
            var textService = new TextService();


            var text = new String(Enumerable.Repeat('D', 55).ToArray());
            var shortenText = textService.ShortenText(text, 40);
            var expectedResult = new string(Enumerable.Repeat('D', 40).ToArray()) + "...";

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldWorkCorrectlyShorterString()
        {
            var textService = new TextService();


            var text = new String(Enumerable.Repeat('D', 20).ToArray());
            var shortenText = textService.ShortenText(text);
            var expectedResult = new string(Enumerable.Repeat('D', 20).ToArray());

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldWorkCorrectlyShorterStringAndGivenMaxLength()
        {
            var textService = new TextService();


            var text = new String(Enumerable.Repeat('D', 20).ToArray());
            var shortenText = textService.ShortenText(text, 200);
            var expectedResult = new string(Enumerable.Repeat('D', 20).ToArray());

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldWorkCorrectlyWhenGivenNull()
        {
            var textService = new TextService();


            var shortenText = textService.ShortenText(null);
            var expectedResult = "";

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldWorkCorrectlyWhenGivenNullAndGivenMaxLength()
        {
            var textService = new TextService();


            var shortenText = textService.ShortenText(null, 200);
            var expectedResult = "";

            Assert.Equal(expectedResult, shortenText);
        }
    }
}
