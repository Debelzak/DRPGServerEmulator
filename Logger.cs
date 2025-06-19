using DRPGServer.Network.Enum;

namespace DRPGServer
{
    public class Logger
    {
        private static readonly object _consoleLock = new();

        public static void Info(string message, ConsoleColor? messageColor = null)
        {
            Print("[ INFO ]", ConsoleColor.Blue, message, messageColor);
        }

        public static void Debug(string message, ConsoleColor? messageColor = null)
        {
            if (ConfigManager.DebugMode)
                Print("[DEBUG!]", ConsoleColor.Magenta, message, messageColor);
        }

        public static void Warn(string message, ConsoleColor? messageColor = null)
        {
            Print("[ WARN ]", ConsoleColor.Yellow, message, messageColor);
        }

        public static void Error(string message, ConsoleColor? messageColor = null)
        {
            Print("[ERROR!]", ConsoleColor.Red, message, messageColor);
        }

        private static void Print(string label, ConsoleColor labelColor, string message, ConsoleColor? messageColor)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            lock (_consoleLock)
            {
                Console.ForegroundColor = labelColor;
                Console.Write(label);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" [{timestamp}] ");
                Console.ResetColor();

                if (messageColor != null)
                    Console.ForegroundColor = (ConsoleColor)messageColor;

                Console.WriteLine(message);

                if (messageColor != null)
                    Console.ResetColor();
            }
        }

        public static void DebugPacket(byte[] buffer, SERVER_TYPE serverType, bool request = false)
        {
            if (!ConfigManager.DebugPackets) return;

            using MemoryStream tempStream = new(buffer);
            using BinaryReader tempReader = new(tempStream);

            ushort packetSignature = tempReader.ReadUInt16();
            ushort packetId = tempReader.ReadUInt16();
            ushort packetSize = tempReader.ReadUInt16();

            string label = request ? "INCOMING PACKET" : "SENDING PACKET";
            ConsoleColor labelColor = request ? ConsoleColor.Red : ConsoleColor.Blue;

            lock (_consoleLock)
            {
                Console.ForegroundColor = labelColor;
                Console.WriteLine($"-- [{label}] [{serverType}] ({packetSize} bytes) ---");

                int width = 16;
                for (int i = 0; i < buffer.Length; i += width)
                {
                    var lineBytes = buffer.Skip(i).Take(width).ToArray();
                    string hex = BitConverter.ToString(lineBytes).Replace("-", " ");
                    string ascii = new string(lineBytes.Select(b => (b >= 32 && b <= 126) ? (char)b : '.').ToArray());

                    Console.WriteLine($"{i:x4}  {hex.PadRight(width * 3)}  {ascii}");
                }

                Console.WriteLine("------------------------------");
                Console.ResetColor();
            }
        }
    }
}
