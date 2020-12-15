namespace AutoPlace.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoPlace.Data.Common.Models;

    public class Vote : BaseDeletableModel<int>
    {
        public string ForUserId { get; set; }

        [InverseProperty("VotesForUser")]
        public ApplicationUser ForUser { get; set; }

        public string VoterId { get; set; }

        [InverseProperty("VotesByUser")]
        public ApplicationUser Voter { get; set; }

        public bool VoteValue { get; set; }
    }
}
