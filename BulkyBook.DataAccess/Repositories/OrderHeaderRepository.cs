using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDBContext _db;
        public OrderHeaderRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDB = _db.OrderHeaders.Find(id);
            if (orderFromDB != null)
            {
                orderFromDB.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDB.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDB = _db.OrderHeaders.Find(id);
            if (orderFromDB != null)
            {
                orderFromDB.PaymentDate = DateTime.Now;
                orderFromDB.SessionId = sessionId;
                orderFromDB.PaymentIntentId = paymentIntentId;
            }
        }
    }
}
