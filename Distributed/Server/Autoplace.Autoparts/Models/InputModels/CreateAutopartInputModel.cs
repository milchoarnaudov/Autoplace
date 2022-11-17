using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Autoparts.Models.InputModels
{
    public class CreateAutopartInputModel : AutopartInputModel
    {
        [Required]
        public int CarId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ConditionId { get; set; }
    }
}
