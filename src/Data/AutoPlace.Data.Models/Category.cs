namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Autoparts = new HashSet<Autopart>();
        }

        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
