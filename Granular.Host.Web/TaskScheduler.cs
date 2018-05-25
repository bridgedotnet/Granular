using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using static Retyped.dom;

namespace Granular.Host
{
    public class TaskScheduler : System.Windows.Threading.ITaskScheduler
    {
        public IDisposable ScheduleTask(TimeSpan timeSpan, Action action)
        {
            var token = window.setTimeout(action, timeSpan.TotalMilliseconds);

            return new Disposable(() => window.clearTimeout(token));
        }
    }
}
