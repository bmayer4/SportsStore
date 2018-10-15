using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        //....https://csharp.christiannagel.com/2017/01/25/expressionbodiedmembers/
        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0) {
                context.Products.Add(product);   //create new product
            } else
            {
                Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null) {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;

                }
            }

            context.SaveChanges();
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == productID);

            if (dbEntry != null) {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
