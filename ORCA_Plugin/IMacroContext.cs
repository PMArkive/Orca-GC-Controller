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

        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetStringContext(string key);
        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void SetStringContext(string key, string value);

        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int? GetIntContext(string key);
        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void SetIntContext(string key, int value);

        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetObjectContext(string key);
        /// <summary>
        /// コマンド間で保持される変数です。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void SetObjectContext(string key, object value);

    }
}
