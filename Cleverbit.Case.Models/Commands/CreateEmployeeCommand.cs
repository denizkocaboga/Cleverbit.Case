using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cleverbit.Case.Models.Requests
{
    public record CreateEmployeeCommand
    {
        [Required]        
        public int RegionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [NotMapped]
        [JsonIgnore]        
        public string NameSurname => $"{Name} {Surname}";
    }
}
