﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Index() => View(_ProductData.GetProducts());

        public IActionResult Edit(int id) => 
            _ProductData.GetProductById(id) is { } product 
                ? View(product) 
                : NotFound();

        public IActionResult Delete(int id) =>
            _ProductData.GetProductById(id) is { } product
                ? View(product)
                : NotFound();
    }
}