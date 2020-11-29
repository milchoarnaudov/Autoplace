namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    public class AutopartCondition : BaseDeletableModel<int>
    {
        public AutopartCondition()
        {
            this.Autoparts = new HashSet<Autopart>();
        }

        public string Name { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
