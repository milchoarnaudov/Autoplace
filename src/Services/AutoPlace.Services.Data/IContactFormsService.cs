namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.Models.ContactForm;

    public interface IContactFormsService : ITransientService
    {
        Task CreateAsync(CreateContactForm contactForm);
    }
}
