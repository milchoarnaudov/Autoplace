namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.ContactForm;
    using Moq;
    using Xunit;

    public class ContactFormsServiceTests
    {
        [Fact]
        public async Task ContactFormsListShouldBeIncreasedWhenAddingNewContactForm()
        {
            var list = new List<ContactForm>();
            var mockRepository = new Mock<IDeletableEntityRepository<ContactForm>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ContactForm>()))
                .Callback((ContactForm favorite) => list.Add(favorite));

            var service = new ContactFormsService(mockRepository.Object);

            await service.CreateAsync(new ContactFormDTO
            {
                Email = "test@test",
                FullName = "testtesttesttest",
                Message = "testtesttesttesttest",
                Topic = "Topictesttesttest",
            });

            await service.CreateAsync(new ContactFormDTO
            {
                Email = "test@test",
                FullName = "testtesttesttest",
                Message = "testtesttesttesttest",
                Topic = "Topictesttesttest2",
            });

            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task ContactFormsListShouldNotBeIncreasedWhenAddingNullContactForm()
        {
            var list = new List<ContactForm>();
            var mockRepository = new Mock<IDeletableEntityRepository<ContactForm>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ContactForm>()))
                .Callback((ContactForm favorite) => list.Add(favorite));

            var service = new ContactFormsService(mockRepository.Object);

            await service.CreateAsync(null);
            await service.CreateAsync(null);

            Assert.Empty(list);
        }
    }
}
