using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderDetailService
    {
        List<OrderDetail> GetByOrderId(int orderId);
        void Add(OrderDetail d);
        void Delete(int orderId, int foodId);
    }
}
