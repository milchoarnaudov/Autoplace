namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.ContactForm;

    public interface IContactFormsService
    {
        Task<int> CreateAsync(CreateContactForm contactForm);
    }
}
