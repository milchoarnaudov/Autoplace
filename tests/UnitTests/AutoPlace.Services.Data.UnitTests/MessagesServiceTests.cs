namespace AutoPlace.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Messages;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Message;
    using Moq;
    using Xunit;

    public class MessagesServiceTests
    {
        public MessagesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(MessageListItemViewModel).Assembly);
        }

        [Fact]
        public async Task MessageContentShouldBeCorrect()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .Callback((Message message) => list.Add(message));

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

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .Callback((Message message) => list.Add(message));

            var service = new MessagesService(mockRepository.Object);

            for (int i = 0; i < 5; i++)
            {
                await service.CreateAsync(new CreateMessageDTO
                {
                    ReceiverId = $"Receiver {i}",
                    SenderId = $"Sender {i}",
                    Content = "test",
                });
            }

            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void ThrowsArgumentNullExceptionWhenReceiverIdsIsNull()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .Callback((Message message) => list.Add(message));

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

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .Callback((Message message) => list.Add(message));

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

        [Fact]
        public async Task MessageDeletionWorksAsExpected()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.Delete(It.IsAny<Message>()))
                .Callback((Message message) => list.Remove(message));

            var service = new MessagesService(mockRepository.Object);

            var initalCountOfMessages = 10;
            var countOfDeletedMessages = 2;

            for (int i = 0; i < initalCountOfMessages; i++)
            {
                list.Add(new Message { Id = i });
            }


            for (int i = 0; i < countOfDeletedMessages; i++)
            {
                await service.DeleteAsync(i);
            }

            Assert.Equal(initalCountOfMessages - countOfDeletedMessages, list.Count);
        }

        [Fact]
        public async Task TrueIsReturnedOnSuccessfulDeletion()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.Delete(It.IsAny<Message>()))
                .Callback((Message message) => list.Remove(message));

            var service = new MessagesService(mockRepository.Object);

            var initalCountOfMessages = 10;

            for (int i = 0; i < initalCountOfMessages; i++)
            {
                list.Add(new Message { Id = i });
            }

            var result = await service.DeleteAsync(1);

            Assert.Equal(initalCountOfMessages - 1, list.Count);
            Assert.True(result);
        }

        [Fact]
        public async Task FalseIsReturnedOnFailedDeletion()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.Delete(It.IsAny<Message>()))
                .Callback((Message message) => list.Remove(message));

            var service = new MessagesService(mockRepository.Object);

            var initalCountOfMessages = 10;

            for (int i = 0; i < initalCountOfMessages; i++)
            {
                list.Add(new Message { Id = i });
            }

            var result = await service.DeleteAsync(initalCountOfMessages + 10);

            Assert.Equal(initalCountOfMessages, list.Count);
            Assert.False(result);
        }

        [Fact]
        public void GetByIdReturnsTheCorrectMessage()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            var service = new MessagesService(mockRepository.Object);

            var messageId = 1;

            list.Add(new Message
            {
                Id = messageId,
                Receiver = new ApplicationUser(),
                Sender = new ApplicationUser(),
                Autopart = new Autopart(),
            });

            var result = service.GetById<MessageListItemViewModel>(messageId);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetByIdReturnsNull()
        {
            var list = new List<Message>();
            var mockRepository = new Mock<IDeletableEntityRepository<Message>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            var service = new MessagesService(mockRepository.Object);

            var messageId = 1;
            var nonExistingId = 2;

            list.Add(new Message
            {
                Id = messageId,
                Receiver = new ApplicationUser(),
                Sender = new ApplicationUser(),
                Autopart = new Autopart(),
            });

            var result = service.GetById<MessageListItemViewModel>(nonExistingId);

            Assert.Null(result);
        }
    }
}
