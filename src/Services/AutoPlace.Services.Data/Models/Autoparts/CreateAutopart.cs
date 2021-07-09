namespace AutoPlace.Services.Data.Models.Autoparts
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class CreateAutopart
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int MakeYear { get; set; }

        public int CarManufacturerId { get; set; }

        public int ModelId { get; set; }

        public int CarTypeId { get; set; }

        public int CategoryId { get; set; }

        public int ConditionId { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
