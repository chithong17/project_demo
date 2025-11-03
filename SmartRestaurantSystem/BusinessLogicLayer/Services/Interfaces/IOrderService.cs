using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Add(Order o);
        void Update(Order o);
        void Delete(int id);
        decimal GetTodayRevenue();

        void AddFoodToOrder(int orderId, int foodId, int quantity);
    }
}
