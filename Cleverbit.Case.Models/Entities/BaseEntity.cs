using System.ComponentModel.DataAnnotations.Schema;

namespace Cleverbit.Case.Models.Entities
{
    public interface IBaseEntity<T> where T : struct
    {
        public T Id { get; set; }
    }
}
