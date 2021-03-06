﻿namespace AutoPlace.Services.Data.Models.Autoparts
{
    public class SearchFilters
    {
        public decimal? MaxPrice { get; set; }

        public int ConditionId { get; set; }

        public int CategoryId { get; set; }

        public int? CarMakeYear { get; set; }

        public int CarManufacturerId { get; set; }

        public int ModelId { get; set; }

        public int CarTypeId { get; set; }
    }
}
