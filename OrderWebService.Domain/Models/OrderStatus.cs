using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebService.Domain.Models
{
    internal enum OrderStatus
    {
        Created,
        OrderAccepted,
        InProgress,
        WaitingDelivery,
        Delivering,
        Completed,
        Cancelled
    }
}
