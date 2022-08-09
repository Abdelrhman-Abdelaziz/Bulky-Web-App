using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncreamentCount(ShoppingCart shoppingCart, int count);
        int DecreamentCount(ShoppingCart shoppingCart, int count);
    }
}
