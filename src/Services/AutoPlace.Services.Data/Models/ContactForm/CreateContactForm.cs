namespace AutoPlace.Services.Data.Models.ContactForm
{
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class CreateContactForm : IMapTo<ContactForm>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Topic { get; set; }

        public string Message { get; set; }
    }
}
