using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Autoparts.Models.InputModels
{
    public class AutopartInputModel
    {
        [Required]
        [StringLength(maximumLength: Constants.AutopartNameMaxLength, MinimumLength = Constants.AutopartNameMinLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.AutopartPriceMinValue, Constants.AutopartPriceMaxValue, ErrorMessage = "The price is either too high or too low")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(maximumLength: Constants.AutopartDescriptionMaxLength, MinimumLength = Constants.AutopartDescriptionMinLength)]
        public string Description { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
