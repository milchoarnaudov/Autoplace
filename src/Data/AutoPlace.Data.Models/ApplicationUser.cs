// ReSharper disable VirtualMemberCallInConstructor
namespace AutoPlace.Data.Models
{
    using System;
    using System.Collections.Generic;

    using AutoPlace.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Autoparts = new HashSet<Autopart>();
            this.CommentsForUser = new HashSet<Comment>();
            this.CommentsByUser = new HashSet<Comment>();
            this.MessagesReceived = new HashSet<Message>();
            this.MessagesSent = new HashSet<Message>();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Autopart> Autoparts { get; set; }

        public virtual ICollection<Comment> CommentsForUser { get; set; }

        public virtual ICollection<Comment> CommentsByUser { get; set; }

        public virtual ICollection<Message> MessagesReceived { get; set; }

        public virtual ICollection<Message> MessagesSent { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
