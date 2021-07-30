using AutoPlace.Tests.Utils;
using System.Threading.Tasks;
using Xunit;

namespace AutoPlace.Services.UnitTests
{
    public class ImageServiceTests
    {
        [Fact]
        public async Task ImageSaveReturnTrueWhenHasAllowedExtension()
        {
            var imageService = new FileSystemImageService();
            var mockImage = MockObjects.GetMockFile("test.png").Object;

            var result = await imageService.Save(mockImage, "/test/", "test");

            Assert.True(result);
        }

        [Fact]
        public async Task ImageSaveReturnFalseWhenHasNotAllowedExtension()
        {
            var imageService = new FileSystemImageService();
            var mockImage = MockObjects.GetMockFile("test.json").Object;

            var result = await imageService.Save(mockImage, "/test/", "test");

            Assert.False(result);
        }

        [Fact]
        public void GetExtensionsMethodReturnsCorrectExtension()
        {
            var imageService = new FileSystemImageService();
            var input = "test.png";
            var expectedResult = "png";

            var result = imageService.GetExtension(input);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetExtensionsMethodReturnsCorrectExtensionWithComplexFileName()
        {
            var imageService = new FileSystemImageService();
            var input = "test-test.test.json";
            var expectedResult = "json";

            var result = imageService.GetExtension(input);

            Assert.Equal(expectedResult, result);
        }
    }
}
