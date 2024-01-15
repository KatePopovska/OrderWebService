using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebService.Contracts.Http.Errors
{
    public class ErrorResponse
    {
        public string[] Errors { get; init; }
    }
}
