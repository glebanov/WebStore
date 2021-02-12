using Microsoft.AspNetCore.Http;
using System;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor)
        {
            this._HttpContextAccessor = HttpContextAccessor;
        }

        public void Add(int id) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public void Decrement(int id) => throw new NotImplementedException();
        public CartViewModel GetViewModel() => throw new NotImplementedException();
        public void Remove(int id) => throw new NotImplementedException();

    }
}
