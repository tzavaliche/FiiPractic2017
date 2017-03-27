using Market.Infrastructure;
using Market.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private Repository repo;

        public ProductsController(Repository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IList<Product> GetProducts()
        {
            return repo.Products;
        }

        [HttpGet("{id}")]
        public Product GetProduct(int id)
        {
            return repo.Products.FirstOrDefault(p => p.Id == id);
        }

        [HttpPost]
        public void AddProduct([FromBody] Product product)
        {
            product.Id = GetNextId();

            repo.Products.Add(product);
        }

        [HttpPut("{id}")]
        public void UpdateProduct(int id, [FromBody] Product product)
        {
            var productFromRepo = this.repo.Products.FirstOrDefault(p => p.Id == id);

            productFromRepo.Name = product.Name;
        }

        private int GetNextId()
        {
            return new Random().Next(100000);
        }
    }
}
