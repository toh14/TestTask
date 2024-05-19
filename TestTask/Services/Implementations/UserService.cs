using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService 
    {
        private readonly ApplicationDbContext? _context;

        public UserService(ApplicationDbContext? context) => _context = context;

        public async Task<User?> GetUser()         //return a User with max total sum of delivered orders at 2003
        {


            DateTime date = new(2003, 1, 1, 0, 0, 0);

            if (_context != null)
                return await _context.Users.Select(u => new
                {
                    User = u,
                    Total = u.Orders.Where(o => o.Status == OrderStatus.Delivered && o.CreatedAt >= date && o.CreatedAt < date.AddYears(1)).Sum(o => o.Price)
                })
                .OrderByDescending(_ => _.Total)  //Sorts in descending
                .Select(_ => _.User)
                .FirstOrDefaultAsync();
            else throw new Exception("Database has no data"); 
        }


        async Task<List<User>> IUserService.GetUsers()  //return Users with paid orders from 2010
        {
            DateTime date = new(2010, 1, 1, 0, 0, 0);

            if (_context != null)
                return await _context.Users.Select(u => new
                {
                    User = u,
                    PaidOrders = u.Orders.Where(o => o.Status == OrderStatus.Paid && o.CreatedAt >= date && o.CreatedAt < date.AddYears(1)).ToList()
                })
                .Where(_ => _.PaidOrders.Count > 0)  //checking paidOrderList for is it not null
                .Select(_ => _.User)
                .ToListAsync();
            else throw new Exception("Database has no data");
        } 
    }
}
