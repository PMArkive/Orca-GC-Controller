using ArduinoAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCController
{
    public interface IPort : IWritable
    {
        void Open(string portName, bool rts, bool dtr);
        bool IsOpen { get; }
        void Close();
    }
    public class MyPort : IPort
    {
        private SerialPort _port;

        public void Write(byte[] buffer, int offset, int count)
            => _port?.Write(buffer, offset, count);
        public void Open(string portName, bool rts, bool dtr)
        {
            if (IsOpen) return;

            if (_port is null) _port = new SerialPort(portName, 4800) { RtsEnable = rts, DtrEnable = dtr };
            _port.Open();
        }
        public bool IsOpen { get => _port?.IsOpen ?? false; }
        public void Close()
            => _port?.Close();
    }

    class DebugPort : IPort
    {
        private StringBuilder _logger;
        public void Write(byte[] buffer, int offset, int count)
            => _logger.AppendLine($"{string.Join(" ", buffer.Select(_ => $"{_:X2}"))}");
        public void Open(string portName, bool rts, bool dtr) => _logger = new StringBuilder();
        public bool IsOpen { get => true; }
        public void Close() => System.IO.File.WriteAllText($"./log.txt", _logger.ToString());
    }
}
