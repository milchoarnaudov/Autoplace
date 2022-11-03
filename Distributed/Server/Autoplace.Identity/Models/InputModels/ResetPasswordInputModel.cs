namespace Autoplace.Identity.Models.InputModels
{
    public class ResetPasswordInputModel : LoginInputModel
    {
        public string Token { get; set; }
    }
}
