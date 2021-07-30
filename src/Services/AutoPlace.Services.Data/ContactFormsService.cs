namespace AutoPlace.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.ContactForm;

    public class ContactFormsService : IContactFormsService
    {
        private readonly IDeletableEntityRepository<ContactForm> contactFormRepository;

        public ContactFormsService(IDeletableEntityRepository<ContactForm> contactFormRepository)
        {
            this.contactFormRepository = contactFormRepository;
        }

        public async Task<int> CreateAsync(CreateContactForm contactForm)
        {
            if (contactForm is null ||
                string.IsNullOrWhiteSpace(contactForm.FullName) ||
                string.IsNullOrWhiteSpace(contactForm.Email) ||
                string.IsNullOrWhiteSpace(contactForm.Message) ||
                string.IsNullOrWhiteSpace(contactForm.Topic))
            {
                return 0;
            }

            var contactFormEntity = new ContactForm
            {
                FullName = contactForm.Email,
                Email = contactForm.Email,
                Topic = contactForm.Topic,
                Message = contactForm.Message,
            };

            await this.contactFormRepository.AddAsync(contactFormEntity);
            await this.contactFormRepository.SaveChangesAsync();

            return contactFormEntity.Id;
        }
    }
}
