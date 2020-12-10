namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class CreateAutopartInputModel : IMapTo<CreateAutopartDTO>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public int MakeYear { get; set; }

        public int CarManufacturerId { get; set; }

        public int ModelId { get; set; }

        public int CarTypeId { get; set; }

        public int CategoryId { get; set; }

        public int ConditionId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CarManufacturers { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CarTypes { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Conditions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
