using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace OrderWebService.Domain.Commands
{
    public class CreateOrderCommand : IRequest<CreateOrderCommandResult>
    {
        public string OrderDetails { get; init; }
    }
}
