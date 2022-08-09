using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
