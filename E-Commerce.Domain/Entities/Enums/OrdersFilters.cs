using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Enums
{
    public enum OrdersFilters
    {
        All = 0,
        Pending = 1,
        Faild = 2,
        Success = 3,
        Today = 4,
        Last30Days = 5
    }
}
