namespace AutoPlace.Services.Data.UnitTests
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

            var createFormCount = 2;

            for (int i = 0; i < createFormCount; i++)
            {
                await service.CreateAsync(new CreateContactFormDTO
                {
                    Email = "test@test",
                    FullName = "test",
                    Message = "test",
                    Topic = "test",
                });
            }

            Assert.Equal(createFormCount, list.Count());
        }
    }
}
