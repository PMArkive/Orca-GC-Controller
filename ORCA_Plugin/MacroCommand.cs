using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using ArduinoAPI;

namespace ORCA_Plugin
{
    public abstract class MacroCommand
    {
        public abstract void Execute(SerialPort port, in CancellationToken token, IMacroContext context);
    }

    public interface IMacroCommandParser<out T>
        where T : MacroCommand
    {
        /// <summary>
        /// 解釈に失敗したらnullを返します。
        /// </summary>
        /// <param name="args"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        T Parse(string[] args, IMacroParseContext context, out string errorMessage);
    }

    public sealed class MacroCommandAttribute : System.Attribute
    {
        /// <summary>
        /// コマンド名として使う文字列です。
        /// 既定のコマンドである PRESS, WAIT, START, HIT は 使うことはできません。
        /// </summary>
        public string CommandName { get; set; }
        public MacroCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
