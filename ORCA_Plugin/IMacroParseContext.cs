using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORCA_Plugin
{
    public interface IMacroParserContext
    {
        /// <summary>
        /// 現在読んでいるマクロの行数です。
        /// </summary>
        int CurrentLine { get; }

        /// <summary>
        /// 指定されたlabelのタイマーが起動済みかどうかを取得します。
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        bool TimerStarted(int label);
        /// <summary>
        /// 指定されたlabelのタイマーを起動したことを表明します。
        /// </summary>
        /// <param name="label"></param>
        void SetTimerStarted(int label);

        /// <summary>
        /// 指定されたlabelのタイマーの{frame}FにHitを計画していることを表明します。
        /// </summary>
        /// <param name="label"></param>
        /// <param name="frame"></param>
        void AddHitPlan(int label, int frame);
    }
}
