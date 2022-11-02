namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BaseAutopartsViewModel
    {
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }

        [Range(1, 1_000_000_000, ErrorMessage = "The price is either too high or too low")]
        [Required]
        public decimal Price { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 5)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Car Model Year")]
        [Required]
        public virtual int CarMakeYear { get; set; }
    }
}
