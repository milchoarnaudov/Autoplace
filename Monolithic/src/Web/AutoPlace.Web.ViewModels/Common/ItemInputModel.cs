namespace AutoPlace.Web.ViewModels.Common
{
    using System.ComponentModel.DataAnnotations;

    public class ItemInputModel
    {
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
    }
}
