namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Comments;
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
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "b",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "c",
                CommentedUserId = "d",
                Content = "testtest",
            });

            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task UserCanCommentHisOwnProfile()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "a",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "a",
                Content = "testtest",
            });

            Assert.Equal(2, list.Where(x => x.CommentedUserId == "a").Count());
        }

        [Fact]
        public async Task UserCanCommentMultipleUsers()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "b",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "b",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "a",
                CommentedUserId = "c",
                Content = "testtest",
            });

            Assert.Equal(3, list.Where(x => x.CommentatorId == "a").Count());
        }

        [Fact]
        public async Task MultipleUsersCanCommentOneUser()
        {
            var list = new List<Comment>();
            var mockRepository = new Mock<IDeletableEntityRepository<Comment>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .Callback((Comment favorite) => list.Add(favorite));

            var service = new CommentsService(mockRepository.Object);

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "b",
                CommentedUserId = "a",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "c",
                CommentedUserId = "a",
                Content = "testtest",
            });

            await service.CreateAsync(new CreateCommentDTO
            {
                CommentatorId = "d",
                CommentedUserId = "a",
                Content = "testtest",
            });

            Assert.Equal(3, list.Where(x => x.CommentedUserId == "a").Count());
        }
    }
}
