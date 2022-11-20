namespace Autoplace.Members.Models.OutputModels
{
    public class ChatForUserOutputModel
    {
        public int Id { get; set; }

        public MemberOutputModel WithMember { get; set; }

        public DateTime? LastInteractionDateTime { get; set; }
    }
}
