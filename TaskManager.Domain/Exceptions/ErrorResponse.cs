using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Exceptions
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;
        public string? Type { get; set; }
        public int StatusCode { get; set; }
    }
}
