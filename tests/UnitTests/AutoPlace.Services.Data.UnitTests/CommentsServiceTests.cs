namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Comments;
    using Moq;
    using Xunit;

    public class CommentsServiceTests
    {
        [Fact]
        public async Task CommentsListShouldBeIncreasedWhenAddingNewComments()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(new CreateComment
            {
                CommentatorId = "a",
                CommentedUserId = "b",
                Content = "Test",
            });

            await service.CreateAsync(new CreateComment
            {
                CommentatorId = "c",
                CommentedUserId = "d",
                Content = "Test",
            });

            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task CommentsAreNotSavedOnIncorrectInput()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(null);

            await service.CreateAsync(new CreateComment
            {
                CommentatorId = null,
                CommentedUserId = null,
                Content = null,
            });

            Assert.Empty(list);
        }

        [Fact]
        public async Task UserCanCommentHisOwnProfile()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            var numberOfComments = 2;

            for (int i = 0; i < numberOfComments; i++)
            {
                await service.CreateAsync(new CreateComment
                {
                    CommentatorId = "a",
                    CommentedUserId = "a",
                    Content = "Test",
                });
            }

            Assert.Equal(numberOfComments, list.Where(x => x.CommentedUserId == "a").Count());
        }

        [Fact]
        public async Task UserCanCommentMultipleUsers()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);
            var commentsCount = 3;

            for (int i = 0; i < commentsCount; i++)
            {
                await service.CreateAsync(new CreateComment
                {
                    CommentatorId = "a",
                    CommentedUserId = $"{i}",
                    Content = "Test",
                });
            }

            Assert.Equal(commentsCount, list.Where(x => x.CommentatorId == "a").Count());
        }

        [Fact]
        public async Task MultipleUsersCanCommentOneUser()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            var commentsCount = 3;

            for (int i = 0; i < commentsCount; i++)
            {
                await service.CreateAsync(new CreateComment
                {
                    CommentatorId = $"{i}",
                    CommentedUserId = "a",
                    Content = "Test",
                });
            }

            Assert.Equal(commentsCount, list.Where(x => x.CommentedUserId == "a").Count());
        }
    }
}
