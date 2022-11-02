namespace AutoPlace.Data.Models
{
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    public class Car : BaseDeletableModel<int>
    {
        public int MakeYear { get; set; }

        public int ModelId { get; set; }

        public virtual CarModel Model { get; set; }

        public int CarTypeId { get; set; }

        public virtual CarType CarType { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }
    }
}
