namespace AutoPlace.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.ContactForm;
    using AutoPlace.Web.ViewModels.ContactForm;
    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        private readonly IContactFormService contactFormService;

        public ContactController(IContactFormService contactFormService)
        {
            this.contactFormService = contactFormService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateContactFormInputModel input)
        {
            await this.contactFormService.Create(new ContactFormDTO
            {
                FullName = input.FullName,
                Email = input.Email,
                Topic = input.Message,
                Message = input.Message,
            });

            return this.Redirect("/");
        }
    }
}
