using System.Threading;

namespace ORCA_Plugin
{
    public interface IMacroContext
    {
        void StartTimer();
        void StartTimer(int label);
        bool Wait(int duration, in CancellationToken token, bool withRestart = true);
        bool Wait(int label, int duration, in CancellationToken token, bool withRestart = true);
        void GetNextHitIndex();
    }
}
