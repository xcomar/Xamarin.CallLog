using App1.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Interfaces
{
    public interface ICallLog
    {
        IEnumerable<CallLogModel> GetCallLogs();
    }
}
