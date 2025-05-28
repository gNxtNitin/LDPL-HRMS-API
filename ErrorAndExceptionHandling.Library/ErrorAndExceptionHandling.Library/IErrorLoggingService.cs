using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorAndExceptionHandling.Library
{
    public interface IErrorLoggingService
    {
        Task LogError(Exception exception, string controller, string action, string userId = null);
    }
}
