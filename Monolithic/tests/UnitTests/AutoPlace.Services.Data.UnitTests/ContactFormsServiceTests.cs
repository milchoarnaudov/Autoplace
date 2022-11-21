namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.ContactForm;
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
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ContactForm>()))
                .Callback((ContactForm favorite) => list.Add(favorite));

            var service = new ContactFormsService(mockRepository.Object);

            var createFormCount = 2;

            for (int i = 0; i < createFormCount; i++)
            {
                await service.CreateAsync(new CreateContactForm
                {
                    Email = "test@test",
                    FullName = "test",
                    Message = "test",
                    Topic = "test",
                });
            }

            Assert.Equal(createFormCount, list.Count());
        }

        [Fact]
        public async Task ContactFormsIsNotSavedOnEmptySpacesInput()
        {
            var list = new List<ContactForm>();
            var mockRepository = new Mock<IDeletableEntityRepository<ContactForm>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ContactForm>()))
                .Callback((ContactForm favorite) => list.Add(favorite));

            var service = new ContactFormsService(mockRepository.Object);

            await service.CreateAsync(new CreateContactForm
            {
                Email = "   ",
                FullName = "   ",
                Message = "   ",
                Topic = "   ",
            });

            await service.CreateAsync(null);

            Assert.Empty(list);
        }

        [Fact]
        public async Task ContactFormsIsNotSavedOnNullInputs()
        {
            var list = new List<ContactForm>();
            var mockRepository = new Mock<IDeletableEntityRepository<ContactForm>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<ContactForm>()))
                .Callback((ContactForm favorite) => list.Add(favorite));

            var service = new ContactFormsService(mockRepository.Object);

            await service.CreateAsync(new CreateContactForm
            {
                Email = null,
                FullName = null,
                Message = null,
                Topic = null,
            });

            await service.CreateAsync(null);

            Assert.Empty(list);
        }
    }
}
