using System.ComponentModel.DataAnnotations;

namespace Cleverbit.Case.Models.Requests
{
    public record CreateRegionCommand
    (
        [Required]
        [StringLength(100)]
        string Name,

        [Required]
        int Id,

        int? ParentId
        )
    { }
}
