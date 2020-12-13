namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.ContactForm;

    public interface IContactFormsService
    {
        Task Create(ContactFormDTO contactForm);
    }
}
