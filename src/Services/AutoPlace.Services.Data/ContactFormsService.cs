namespace AutoPlace.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.ContactForm;

    public class ContactFormsService : IContactFormsService
    {
        private readonly IDeletableEntityRepository<ContactForm> contactFormRepository;

        public ContactFormsService(IDeletableEntityRepository<ContactForm> contactFormRepository)
        {
            this.contactFormRepository = contactFormRepository;
        }

        public async Task CreateAsync(CreateContactFormDTO contactForm)
        {
            if (contactForm == null)
            {
                return;
            }

            await this.contactFormRepository.AddAsync(new ContactForm
            {
                 FullName = contactForm.Email,
                 Email = contactForm.Email,
                 Topic = contactForm.Topic,
                 Message = contactForm.Message,
            });

            await this.contactFormRepository.SaveChangesAsync();
        }
    }
}
