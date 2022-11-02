namespace AutoPlace.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoPlace.Data.Common.Models;

    public class ContactForm : BaseDeletableModel<int>
    {
        [MaxLength(255)]
        [Required]
        public string FullName { get; set; }

        [MaxLength(255)]
        [Required]
        public string Email { get; set; }

        [MaxLength(100)]
        [Required]
        public string Topic { get; set; }

        [MaxLength(500)]
        [Required]
        public string Message { get; set; }
    }
}
