namespace AutoPlace.Tests.Utils
{
    using System.IO;

    using Microsoft.AspNetCore.Http;
    using Moq;

    public static class MockObjects
    {
        public static Mock<IFormFile> GetMockFile(string fileName = "test.png")
        {
            var fileMock = new Mock<IFormFile>();
            var content = "test";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock;
        }
    }
}
