using System.ComponentModel.DataAnnotations;

namespace Autoplace.Autoparts.Models.InputModels
{
    public class EditAutopartInputModel : AutopartInputModel
    {
        [Required]
        public int Id { get; set; }
    }
}
