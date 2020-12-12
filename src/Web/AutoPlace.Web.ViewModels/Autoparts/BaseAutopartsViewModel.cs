namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System.ComponentModel.DataAnnotations;

    public class BaseAutopartsViewModel
    {
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }

        [Range(1, 1_000_000_000)]
        [Required]
        public decimal Price { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 5)]
        [Required]
        public string Description { get; set; }

        [Range(1960, 2020)]
        public int MakeYear { get; set; }
    }
}
