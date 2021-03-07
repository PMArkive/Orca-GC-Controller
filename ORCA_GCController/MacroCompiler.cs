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
    class MacroScript
    {
        class PressCommandArgs
        {
            public IEnumerable<ControllerInput> Buttons;
            public int Duration = 200;
            public int Interval = 0;
            public int StopWatchLabel = -1;

            public bool IsValid = true;
            public string ErrorMessage;

            private readonly char[] optionCharactors = new char[] { 'd', 'i', 's' };
            public void Convert(string[] args)
            {
                if (args.Length == 0 || args.Length > 4)
                {
                    IsValid = false;
                    ErrorMessage = "引数の数が不正です";

                    return;
                }

                // 第1引数はボタン系列指定.
                var buttonSymbols = args[0].Split(',');
                if (buttonSymbols.Length == 0)
                {
                    IsValid = false;
                    ErrorMessage = "第1引数(ボタン指定)が空です";

                    return;
                }

                if (buttonSymbols.Any(_ => !buttonMap.ContainsKey(_)))
                {
                    IsValid = false;
                    ErrorMessage = "第1引数(ボタン指定)に不正な文字が含まれています";

                    return;
                }

                Buttons = buttonSymbols.Select(_ => buttonMap[_]);

                // 許可されているオプション引数は-d, -i, -s
                var options = optionCharactors.ToDictionary(_ => _, _ => "");
                Console.WriteLine(String.Join(",", args));
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].Length < 4 || !Regex.IsMatch(args[i].Substring(0, 3), $"-[{string.Join(",", optionCharactors)}]="))
                    {
                        IsValid = false;
                        ErrorMessage = "オプション指定子が不正です";

                        return;
                    }

                    if (options[args[i][1]] != "")
                    {
                        IsValid = false;
                        ErrorMessage = "オプションが重複しています";

                        return;
                    }

                    options[args[i][1]] = args[i].Substring(3);
                }

                // Duration
                if (options['d'] != "")
                {
                    if (!int.TryParse(options['d'], out Duration) || Duration < 0)
                    {
                        IsValid = false;
                        ErrorMessage = "-dオプションは32bit符号あり整数に収まる負でない数値である必要があります";

                        return;
                    }
                }

                // Interval
                if (options['i'] != "")
                {
                    if (!int.TryParse(options['i'], out Interval) || Interval < 0)
                    {
                        IsValid = false;
                        ErrorMessage = "-iオプションは32bit符号あり整数に収まる負でない数値である必要があります";

                        return;
                    }
                }

                // 開始するタイマーのラベル.
                // 起動の重複は外側で検知する.
                if (options['s'] != "")
                {
                    if (!int.TryParse(options['s'], out StopWatchLabel) || options['s'].Length != 1)
                    {
                        IsValid = false;
                        ErrorMessage = "-sオプションは1桁の数字である必要があります";

                        return;
                    }
                }
            }
        }

        class WaitCommandArgs
        {
            public int DurationMillisecond = 0;
            public bool IsValid = true;
            public string ErrorMessage;

            public void Convert(string[] args)
            {
                if (args.Length != 1)
                {
                    IsValid = false;
                    ErrorMessage = "引数の数が不正です";

                    return;
                }

                // 第1引数はボタン系列指定.
                if (!int.TryParse(args[0], out DurationMillisecond) || DurationMillisecond < 0)
                {
                    IsValid = false;
                    ErrorMessage = "第1引数(待機時間[ms]指定)は32bit符号あり整数に収まる負でない数値である必要があります";

                    return;
                }
            }
        }

        class StartCommandArgs
        {
            public int StopWatchLabel = 0;

            public bool IsValid = true;
            public string ErrorMessage;
            public void Convert(string[] args)
            {
                if (args.Length > 1)
                {
                    IsValid = false;
                    ErrorMessage = "引数の数が不正です";

                    return;
                }

                if (args.Length == 1)
                {
                    if (args[0].Substring(0, 3) != "-s=")
                    {
                        IsValid = false;
                        ErrorMessage = "オプション指定子が不正です";

                        return;
                    }

                    var val = args[0].Substring(3);
                    if (val.Length != 1 || !int.TryParse(val, out StopWatchLabel))
                    {
                        IsValid = false;
                        ErrorMessage = "-sオプションは1桁の数字である必要があります";
                    }

                }
            }
        }

        class HitCommandArgs
        {
            public ControllerInput Button = ControllerInput.KeysAllUp;
            public int Frame = -1;
            public int StopWatchLabel = 0;
            public int Duration = 200;
            public int StartStopWatchLabel = -1;

            public bool IsValid = true;
            public string ErrorMessage;

            private readonly char[] optionCharactors = new char[] { 'l', 'd', 's', 'c' };
            public void Convert(string[] args)
            {
                if (args.Length < 2 || args.Length > 5)
                {
                    IsValid = false;
                    ErrorMessage = "引数の数が不正です";

                    return;
                }

                // 第1引数はボタン指定.
                if (!buttonMap.ContainsKey(args[0]))
                {
                    IsValid = false;
                    ErrorMessage = "第1引数(ボタン指定)に不正な文字が含まれています";

                    return;
                }
                Button = buttonMap[args[0]];

                // 第2引数はフレーム.
                if (!int.TryParse(args[1], out Frame) || Frame < 0)
                {
                    IsValid = false;
                    ErrorMessage = "第2引数(フレーム指定)は32bit符号あり整数に収まる負でない数値である必要があります";

                    return;
                }

                // 許可されているオプション引数は-l, -d, -s, -c
                var options = optionCharactors.ToDictionary(_ => _, _ => "");
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i].Length < 4 || !Regex.IsMatch(args[i].Substring(0, 3), $"-[{string.Join(",", optionCharactors)}]="))
                    {
                        IsValid = false;
                        ErrorMessage = "オプション指定子が不正です";

                        return;
                    }

                    if (options[args[i][1]] != "")
                    {
                        IsValid = false;
                        ErrorMessage = "オプションが重複しています";

                        return;
                    }

                    options[args[i][1]] = args[i].Substring(3);
                }

                // Duration
                if (options['d'] != "")
                {
                    if (!int.TryParse(options['d'], out Duration) || Duration < 0)
                    {
                        IsValid = false;
                        ErrorMessage = "-dオプションは32bit符号あり整数に収まる負でない数値である必要があります";

                        return;
                    }
                }

                // Label
                if (options['l'] != "")
                {
                    if (!int.TryParse(options['l'], out StopWatchLabel) || options['l'].Length != 1)
                    {
                        IsValid = false;
                        ErrorMessage = "-lオプションは1桁の数字である必要があります";

                        return;
                    }
                }

                if(options['c'] != "")
                {
                    if (!int.TryParse(options['c'], out int correct))
                    {
                        IsValid = false;
                        ErrorMessage = "-cオプションは32bit符号あり整数に収まる数値である必要があります";

                        return;
                    }
                    Frame = Math.Max(0, Frame + correct);
                }

                // 開始するタイマーのラベル.
                // 起動の重複は外側で検知する.
                if (options['s'] != "")
                {
                    if (!int.TryParse(options['s'], out StartStopWatchLabel) || options['s'].Length != 1)
                    {
                        IsValid = false;
                        ErrorMessage = "-sオプションは1桁の数字である必要があります";

                        return;
                    }
                }
            }
        }


        private readonly static SortedSet<string> commandKeyWord = new SortedSet<string>(new string[] 
        {
            "Press",
            "Wait",
            "Hit",
            "Start" 
        });
        private readonly static Dictionary<string, ControllerInput> buttonMap = new Dictionary<string, ControllerInput>()
        {
            { "A", ControllerInput.A },
            { "B", ControllerInput.B },
            { "Z", ControllerInput.Z },
            { "St", ControllerInput.Start },
            { "Sl", ControllerInput.Y },
            { "L", ControllerInput.L },
            { "R", ControllerInput.R },
            { "dU", ControllerInput.Up },
            { "dD", ControllerInput.Down },
            { "dL", ControllerInput.Left },
            { "dR", ControllerInput.Right },
            { "tl", ControllerInput.Start | ControllerInput.Y }
        };

        public static MacroScript Compile(string[] macroLines)
        {
            var timerStarted = new bool[10];
            var macro = new MacroScript();
            var existsCommand = false;

            for (int i = 0; i < macroLines.Length; i++)
            {
                var line = macroLines[i];
                if (line.Length == 0) continue; // 空行.
                if (line[0] == '#') continue; // コメント行.

                var args = line.Replace(", ", ",").Split();
                var commandName = args[0];
                if (!commandKeyWord.Contains(commandName))
                    throw new Exception($"[{i + 1}行目] コマンド名が不正です:{commandName}");

                // 先頭を除去する.
                {
                    var temp = new string[args.Length - 1];
                    for (int k = 0; k < temp.Length; k++) temp[k] = args[k + 1];
                    args = temp;
                }

                if(commandName == "Press")
                {
                    var commandArgs = new PressCommandArgs();
                    commandArgs.Convert(args);
                    if (!commandArgs.IsValid)
                        throw new Exception($"[{i + 1}行目] Pressコマンド " + commandArgs.ErrorMessage);

                    // タイマーの重複起動を弾く.
                    if (commandArgs.StopWatchLabel != -1)
                    {
                        if (timerStarted[commandArgs.StopWatchLabel])
                            throw new Exception($"[{i + 1}行目] タイマー{commandArgs.StopWatchLabel}の開始命令が重複しています");

                        timerStarted[commandArgs.StopWatchLabel] = true;
                    }

                    Console.WriteLine(commandArgs.StopWatchLabel);
                    foreach (var b in commandArgs.Buttons)
                        macro.AddPressCommand(b, commandArgs.Duration, commandArgs.Interval, commandArgs.StopWatchLabel);

                    existsCommand = true;
                }
                if(commandName == "Wait")
                {
                    var commandArgs = new WaitCommandArgs();
                    commandArgs.Convert(args);
                    if (!commandArgs.IsValid)
                        throw new Exception($"[{i + 1}行目] Waitコマンド " + commandArgs.ErrorMessage);

                    macro.AddWaitCommand(commandArgs.DurationMillisecond);
                    existsCommand = true;
                }
                if(commandName == "Hit")
                {
                    var commandArgs = new HitCommandArgs();
                    commandArgs.Convert(args);
                    if (!commandArgs.IsValid)
                        throw new Exception($"[{i + 1}行目] Hitコマンド " + commandArgs.ErrorMessage);


                    if (!timerStarted[commandArgs.StopWatchLabel])
                        throw new Exception($"[{i + 1}行目] タイマー[{commandArgs.StopWatchLabel}] Hitコマンドはタイマー起動より後に書かれる必要があります");
                    
                    // タイマーの重複起動を弾く.
                    if (commandArgs.StartStopWatchLabel != -1)
                    {
                        if (timerStarted[commandArgs.StartStopWatchLabel])
                            throw new Exception($"[{i + 1}行目] タイマー{commandArgs.StartStopWatchLabel}の開始命令が重複しています");

                        timerStarted[commandArgs.StartStopWatchLabel] = true;
                    }

                    macro.AddHitCommand(commandArgs.Button, commandArgs.Frame, commandArgs.StopWatchLabel, commandArgs.Duration, commandArgs.StartStopWatchLabel);

                    existsCommand = true;

                }
                if (commandName == "Start")
                {
                    var commandArgs = new StartCommandArgs();
                    commandArgs.Convert(args);
                    if (!commandArgs.IsValid)
                        throw new Exception($"[{i + 1}行目] Startコマンド " + commandArgs.ErrorMessage);

                    // タイマーの重複起動を弾く.
                    if (commandArgs.StopWatchLabel != -1)
                    {
                        if (timerStarted[commandArgs.StopWatchLabel])
                            throw new Exception($"[{i + 1}行目] タイマー{commandArgs.StopWatchLabel}の開始命令が重複しています");

                        timerStarted[commandArgs.StopWatchLabel] = true;
                    }

                    macro.AddStartCommand(commandArgs.StopWatchLabel);
                    existsCommand = true;
                }

            }

            if (!existsCommand) throw new Exception("有効なコマンドがありませんでした");

            return macro;
        }

        private MacroScript()
        {
            commandList = new List<Action<SerialPort, CancellationToken>>();
            stopwatches = Enumerable.Range(0, 11).Select(_ => new Stopwatch()).ToArray();
        }

        private const double fps = 59.7275;
        private readonly List<Action<SerialPort, CancellationToken>> commandList;
        private readonly Stopwatch[] stopwatches; // 0 - 9はlabel指定, 10はWaitコマンド用.
        private readonly List<(int, int)> hitFrames = new List<(int, int)>();

        public int CurrentLine { get; private set; } = -1;
        public int CurrentHitIndex { get; private set; } = -1;
        public int CurrentFrame(int label) => (int)(stopwatches[label].ElapsedMilliseconds * fps / 1000);
        public (int,int)[] HitFrames { get { return hitFrames.ToArray(); } }

        private void AddPressCommand(ControllerInput button, int duration, int interval, int startLabel = -1)
        {
            commandList.Add((port, token) =>
            {
                if (startLabel != -1)
                {
                    stopwatches[startLabel].Restart();
                    if (CurrentHitIndex < 0) CurrentHitIndex = 0;
                }
                port.PressButton(button, duration);
                if (token.IsCancellationRequested) return;
                if (interval > 0) Thread.Sleep(interval);
            });
        }
        private void AddWaitCommand(int millisecondsTimeout)
        {
            commandList.Add((port, token) => 
            {
                stopwatches[10].Restart();
                while(stopwatches[10].ElapsedMilliseconds < millisecondsTimeout && !token.IsCancellationRequested) { }
            });
        }
        private void AddHitCommand(ControllerInput button, int frame, int label, int duration, int startLabel = -1)
        {
            var border = frame * 1000 / fps;
            hitFrames.Add((label, frame));
            commandList.Add((port, token) =>
            {
                while (stopwatches[label].ElapsedMilliseconds < border)
                {
                    if (token.IsCancellationRequested) return;
                }
                CurrentHitIndex++;
                if (startLabel != -1) 
                {
                    stopwatches[startLabel].Restart();
                    if (CurrentHitIndex < 0) CurrentHitIndex = 0;
                }
                port.PressButton(button, duration);
            });
        }
        private void AddStartCommand(int label)
        {
            commandList.Add((port, token) => { CurrentHitIndex++; stopwatches[label].Restart(); });
        }
        
        public Task RunOnceAsync(SerialPort port, CancellationToken token)
        {
            return Task.Run(() =>
            {
                CurrentHitIndex = -1;
                for (int i = 0; i < commandList.Count && !token.IsCancellationRequested; i++)
                {
                    CurrentLine++;
                    commandList[i](port, token);
                }
                CurrentHitIndex = CurrentLine = -1;
            }, token);
        }
        public Task RunLoopAsync(SerialPort port, CancellationToken token)
        {
            return Task.Run(() =>
            {
                CurrentHitIndex = -1;
                while (!token.IsCancellationRequested)
                {
                    for (int i = 0; i < commandList.Count && !token.IsCancellationRequested; i++)
                    {
                        CurrentLine++;
                        commandList[i](port, token);
                    }
                    CurrentHitIndex = CurrentLine = -1;
                }
                CurrentHitIndex = CurrentLine = -1;
            }, token);
        }
    }
}
