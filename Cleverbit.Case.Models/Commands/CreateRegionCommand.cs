using System.ComponentModel.DataAnnotations;

namespace Cleverbit.Case.Models.Requests
{
    public record CreateRegionCommand
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Id { get; set; }
        public int? ParentId { get; set; }
    }
}
