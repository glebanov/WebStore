using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace WebStore.Domain.Entities
{
    //[Table("Brands")]
    public class Brand : NamedEntity, IOrderedEntity
    {
      //  [Column("BrandOrder")]
        public int Order { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
