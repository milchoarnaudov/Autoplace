namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.ContactForm;

    public interface IContactFormService
    {
        Task Create(ContactFormDTO contactForm);
    }
}
