using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.ValueObjects
{
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3
    }
}
