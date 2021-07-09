namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Services.Data.Models.Autoparts;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.Infrastructure.ValidationAttributes;
    using Microsoft.AspNetCore.Http;

    public class CreateAutopartInputModel : BaseAutopartsViewModel, IMapTo<CreateAutopart>
    {
        [Display(Name = "Car Model Year")]
        [ValidateYear(1960)]
        public override int CarMakeYear { get; set; }

        [Display(Name = "Car Manufacturer")]
        [Required]
        public int CarManufacturerId { get; set; }

        [Display(Name = "Car Model")]
        [Required]
        public int ModelId { get; set; }

        [Display(Name = "Car Type")]
        [Required]
        public int CarTypeId { get; set; }

        [Display(Name = "Autopart Category")]
        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Autopart Condition")]
        [Required]
        public int ConditionId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CarManufacturers { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CarTypes { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Conditions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        [Required]
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
