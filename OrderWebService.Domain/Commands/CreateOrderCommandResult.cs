using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace OrderWebService.Domain.Commands
{
    public class CreateOrderCommandResult
    {
        public string OrderId { get; init; }
    }
}
