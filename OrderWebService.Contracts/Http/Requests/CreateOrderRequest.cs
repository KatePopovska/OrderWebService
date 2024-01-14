using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebService.Contracts.Http.Requests
{
    public class CreateOrderRequest
    {
        public string Details { get; init; }
    }
}
