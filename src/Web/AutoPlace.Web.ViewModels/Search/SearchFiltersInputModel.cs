namespace AutoPlace.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SearchFiltersInputModel
    {
        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Autopart Condition")]
        [Required]
        public int ConditionId { get; set; }

        [Display(Name = "Autopart Category")]
        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Car Manufacturer")]
        [Required]
        public int CarManufacturerId { get; set; }

        [Display(Name = "Car Model")]
        [Required]
        public int ModelId { get; set; }

        [Display(Name = "Car Type")]
        public int CarTypeId { get; set; }

        [Display(Name = "Car Model Year (Optional)")]
        [Range(0, 2020)]
        public int? CarMakeYear { get; set; }

        public IEnumerable<KeyValuePair<int, string>> CarManufacturers { get; set; }

        public IEnumerable<KeyValuePair<int, string>> CarTypes { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Conditions { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Categories { get; set; }
    }
}
