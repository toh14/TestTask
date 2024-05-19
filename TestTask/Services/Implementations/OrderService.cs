using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService 
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context) 
            => _context = context;

        public async Task<Order?> GetOrder() //return the newest order ich have more than one item
        {
            if (_context != null)
                return await _context.Orders.Where(o => o.Quantity > 1)
                                            .OrderByDescending(_ => _.CreatedAt)
                                            .FirstOrDefaultAsync();
            else throw new Exception("Database has no data");
        }

        public async Task<List<Order>> GetOrders() //return orders from active users and sorted by creation date
        {
            if (_context != null)
                return await _context.Orders.Select(o => new
                {
                    Order = o,
                    Status = o.User.Status
                })
                .Where(_ => _.Status == UserStatus.Active)
                .Select(_ => _.Order)
                .OrderBy(_ => _.CreatedAt)
                .ToListAsync();
            else throw new Exception("Database has no data");
        }
    }
}
