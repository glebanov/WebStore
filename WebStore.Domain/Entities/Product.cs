using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int SectionId { get; set; }

        //Создаем навигационное свойство
        [ForeignKey(nameof(SectionId))] // внешний ключ 
        public  Section Section { get; set; }

        public int? BrandId { get; set; }

        //Создаем навигационное свойство
       [ForeignKey(nameof(BrandId))]// внешний ключ
        public  Brand Brand { get; set; }

        public string ImageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")] //Тип данных для базы данных
        public decimal Price { get; set; }
    }
}
