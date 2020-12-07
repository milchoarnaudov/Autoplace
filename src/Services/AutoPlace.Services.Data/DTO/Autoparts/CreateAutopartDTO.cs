namespace AutoPlace.Services.Data.DTO.Autoparts
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public class CreateAutopartDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime MakeDate { get; set; }

        public int CarManufacturerId { get; set; }

        public int ModelId { get; set; }

        public int CarTypeId { get; set; }

        public int CategoryId { get; set; }

        public int ConditionId { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
    }
}
