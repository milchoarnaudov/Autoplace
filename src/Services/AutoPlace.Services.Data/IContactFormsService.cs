namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.ContactForm;

    public interface IContactFormsService
    {
        Task CreateAsync(CreateContactForm contactForm);
    }
}
