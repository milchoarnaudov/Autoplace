namespace AutoPlace.Web.ViewModels.Search
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoPlace.Web.ViewModels.Autoparts;

    public class SearchResultsViewModel : AutopartsListViewModel
    {
        public SearchFiltersInputModel SearchFilters { get; set; }
    }
}
