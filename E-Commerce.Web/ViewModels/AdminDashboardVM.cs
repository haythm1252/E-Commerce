using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Web.ViewModels
{
    public class AdminDashboardVM
    {
        public decimal TotalSalesToday { get; set; }
        public decimal TotalSalesLast30Days { get; set; }
        public PagedResult<OrderVM> TodayOrders { get; set; }
        public PagedResult<OrderVM> Last30DaysOrders { get; set; }
    }
}
