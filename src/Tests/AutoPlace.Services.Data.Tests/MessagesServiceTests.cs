namespace AutoPlace.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Messages;
    using Moq;
    using Xunit;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task MessageContentShouldBeCorrect()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback(
                (Message message) => list.Add(message));
            var service = new MessagesService(mockRepository.Object);

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "a",
                SenderId = "b",
                Content = "test",
            });

            Assert.Equal("test", list.FirstOrDefault(x => x.ReceiverId == "a" && x.SenderId == "b")?.Content);
        }

        [Fact]
        public async Task MessageListShouldIncreaseWhenMultipleMessagesAreSent()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback(
                (Message message) => list.Add(message));
            var service = new MessagesService(mockRepository.Object);

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "a",
                SenderId = "b",
                Content = "test",
            });

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "c",
                SenderId = "d",
                Content = "test",
            });

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "c",
                SenderId = "d",
                Content = "test2",
            });

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "c",
                SenderId = "d",
                Content = "test",
            });

            await service.CreateAsync(new CreateMessageDTO
            {
                ReceiverId = "d",
                SenderId = "c",
                Content = "test",
            });

            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenReceiverIdsIsNull()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback(
                (Message message) => list.Add(message));
            var service = new MessagesService(mockRepository.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                service.CreateAsync(new CreateMessageDTO
                {
                    ReceiverId = null,
                    SenderId = "b",
                    Content = "test",
                    AutopartId = 1,
                }).GetAwaiter().GetResult();
            });
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenSenderIdsIsNull()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Message>())).Callback(
                (Message message) => list.Add(message));
            var service = new MessagesService(mockRepository.Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                service.CreateAsync(new CreateMessageDTO
                {
                    ReceiverId = "a",
                    SenderId = null,
                    Content = "test",
                    AutopartId = 1,
                }).GetAwaiter().GetResult();
            });
        }
    }
}
