namespace AutoPlace.Services.Data.DTO.ContactForm
{
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class CreateContactFormDTO : IMapTo<ContactForm>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Topic { get; set; }

        public string Message { get; set; }
    }
}
