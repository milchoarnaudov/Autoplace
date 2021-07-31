namespace AutoPlace.Services.Tests
{
    using AutoPlace.Common;
    using System;
    using System.Linq;
    using Xunit;

    public class TextServiceTests
    {
        [Fact]
        public void WhenTextLengthIsLessThanDefaultLengthTextIsNotShorten()
        {
            var textService = new TextService();

            var text = new string(Enumerable.Repeat('D', GlobalConstants.ShortenTextDefaultLength - 1).ToArray());
            var shortenText = textService.ShortenText(text, GlobalConstants.ShortenTextDefaultLength);
            var expectedResult = text;

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void WhenTextLengthIsBiggerThanDefaultLengthTextIsShorten()
        {
            var textService = new TextService();

            var text = new String(Enumerable.Repeat('D', 55).ToArray());
            var shortenText = textService.ShortenText(text, 40);
            var expectedResult = $"{new string(Enumerable.Repeat('D', 40).ToArray())}...";

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void WhenGivenMaxLengthIsBiggerThanTextLengthTextIsNotShorten()
        {
            var textService = new TextService();

            var text = new String(Enumerable.Repeat('D', 20).ToArray());
            var shortenText = textService.ShortenText(text, 200);
            var expectedResult = text;

            Assert.Equal(expectedResult, shortenText);
        }

        [Fact]
        public void ShortenTextShouldReturnNullWhenInputIsNull()
        {
            var textService = new TextService();

            var shortenText = textService.ShortenText(null, GlobalConstants.ShortenTextDefaultLength);

            Assert.Null(shortenText);
        }

        [Fact]
        public void ShortenTextShouldReturnNullWhenGivenNullAndGivenMaxLength()
        {
            var textService = new TextService();

            var shortenText = textService.ShortenText(null, 200);

            Assert.Null(shortenText);
        }
    }
}
