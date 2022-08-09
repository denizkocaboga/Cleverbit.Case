using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cleverbit.Case.Models.Entities
{
    public class Region : IBaseEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int? ParentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
