namespace AutoPlace.Data.Models
{
    using AutoPlace.Data.Common.Models;

    public class Favorite : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int AutopartId { get; set; }

        public Autopart Autopart { get; set; }
    }
}
