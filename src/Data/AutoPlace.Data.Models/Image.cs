namespace AutoPlace.Data.Models
{
    using System;

    using AutoPlace.Data.Common.Models;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int AutopartId { get; set; }

        public virtual Autopart Autopart { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }

        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}
