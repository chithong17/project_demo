using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        List<OrderDetail> GetByOrderId(int orderId);
        void Add(OrderDetail detail);
        void Delete(int orderId, int foodId);

        List<OrderDetail> GetAll();
    }
}
