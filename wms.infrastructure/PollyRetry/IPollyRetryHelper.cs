using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wms.infrastructure.PollyRetry
{
    public interface IPollyRetryHelper
    {
        Task<T> Retry<T>(Func<Task<T>> action);
    }
}
