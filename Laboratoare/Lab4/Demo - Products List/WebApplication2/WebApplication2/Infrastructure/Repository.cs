using Market.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Infrastructure
{
    public class Repository
    {
        public IList<Product> Products { get; }

        public Repository()
        {
            Products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "beer"
                },

                new Product
                {
                    Id = 2,
                    Name = "water"
                }
            };
        }
    }
}
