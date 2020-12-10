namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    public class AutopartCategory : BaseDeletableModel<int>, IItemEntity
    {
        public AutopartCategory()
        {
            this.Autoparts = new HashSet<Autopart>();
        }

        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
