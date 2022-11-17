namespace Autoplace.Autoparts.Models.InputModels
{
    public class SearchFiltersInputModel
    {
        public string Name { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? ConditionId { get; set; }

        public int? CategoryId { get; set; }

        public int? CarManufacturerId { get; set; }

        public int? CarModelId { get; set; }

        public int? CarTypeId { get; set; }

        public int? PageSize { get; set; }

        public int? Page { get; set; }
    }
}
