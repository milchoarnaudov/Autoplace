namespace AutoPlace.Web.ViewModels.Common
{
    using System;

    public class PaginationViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.AutopartsCount / this.ItemsPerPage);

        public int AutopartsCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
