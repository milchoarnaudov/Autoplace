namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class CreateAutopartInputModel : BaseAutopartsViewModel, IMapTo<CreateAutopartDTO>
    {
        [Required]
        public int CarManufacturerId { get; set; }

        [Required]
        public int ModelId { get; set; }

        [Required]
        public int CarTypeId { get; set; }

        [Required]
        public int CategoryId { get; set; }

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
