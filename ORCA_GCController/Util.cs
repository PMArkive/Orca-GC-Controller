using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GCController.SerialCommunicationWrapper;

namespace GCController
{
    static class UtilExtensions
    {

        internal static bool CancelableWait(this Stopwatch sw, int wait_ms, CancellationToken token, bool withRestart = true)
        {
            if (withRestart) sw.Restart();
            while (sw.ElapsedMilliseconds < wait_ms) if (token.IsCancellationRequested) return true;
            return false;
        }
    }
}
