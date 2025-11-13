using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IDashboardService
    {
        DashboardSummary GetDashboardSummary();
        List<TopFoodDTO> GetTopSellingFoods(int count);
        List<DailyRevenueDTO> GetRevenueByLast7Days();
    }
}
