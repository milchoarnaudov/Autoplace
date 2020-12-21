namespace AutoPlace.Web.ViewModels.Autoparts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoPlace.Web.ViewModels.Common;

    public class AutopartsListViewModel : PaginationViewModel
    {
        public IEnumerable<AutopartsListItemViewModel> Autoparts { get; set; }
    }
}
