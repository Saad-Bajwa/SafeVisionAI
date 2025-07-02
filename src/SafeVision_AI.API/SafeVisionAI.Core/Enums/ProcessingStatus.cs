using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Core.Enums
{
    public enum ProcessingStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}
