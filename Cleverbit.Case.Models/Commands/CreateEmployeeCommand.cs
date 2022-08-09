using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cleverbit.Case.Models.Requests
{
    public record CreateEmployeeCommand
    (
        [Required]
        int RegionId,

        [Required]
        [StringLength(100)]
        string Name,

        [Required]
        [StringLength(100)]
        string Surname
    )
    {

        [NotMapped]
        [JsonIgnore]
        public string NameSurname => $"{Name} {Surname}";
    }
}
