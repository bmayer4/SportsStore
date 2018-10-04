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
    }
}
