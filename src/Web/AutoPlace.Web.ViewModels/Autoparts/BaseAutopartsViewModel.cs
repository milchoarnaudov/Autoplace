namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Web.Infrastructure.ValidationAttributes;

    public class BaseAutopartsViewModel
    {
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        [Required]
        public string Name { get; set; }

        [Range(1, 1_000_000_000, ErrorMessage = "The price is either too high or too low")]
        [Required]
        public decimal Price { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 5)]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Car Model Year")]
        [ValidateYear(1960)]
        public int CarMakeYear { get; set; }
    }
}
