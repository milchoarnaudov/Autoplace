namespace AutoPlace.Web.Controllers
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.ContactForm;
    using AutoPlace.Web.ViewModels.ContactForm;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : BaseController
    {
        private readonly IContactFormsService contactFormService;

        public ContactsController(IContactFormsService contactFormService)
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
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.contactFormService.CreateAsync(new CreateContactFormDTO
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
