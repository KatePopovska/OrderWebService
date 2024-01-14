using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebService.Domain.Models.Events
{
    internal record OrderCreatedEvent
    {
        public string Details { get; init; }
    }
}
