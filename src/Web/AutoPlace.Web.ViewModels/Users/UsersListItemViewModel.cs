namespace AutoPlace.Web.ViewModels.Users
{
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class UsersListItemViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }
    }
}
