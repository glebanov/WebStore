using WebStore.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Mo;yj не писать. entity framework зделает самостоятельно
        public int Id { get; set; }
    }
}
