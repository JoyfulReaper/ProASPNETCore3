using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly StoreDbContext _ctx;

        public IQueryable<Order> Orders => _ctx.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public EFOrderRepository(StoreDbContext ctx)
        {
            _ctx = ctx;
        }
     
        public void SaveOrder(Order order)
        {
            _ctx.AttachRange(order.Lines.Select(l => l.Product));
            if(order.OrderId == 0)
            {
                _ctx.Orders.Add(order);
            }
            _ctx.SaveChanges();
        }
    }
}
