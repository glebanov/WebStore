using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Interfaces
{
    public interface ICartService
    {
        void Add(int id); //Добавление товара

        void Decrement(int id); //Уменьшение товара

        void Remove(int id); //Удаление товара

        void Clear();

        CartViewModel GetViewModel();
    }
}
