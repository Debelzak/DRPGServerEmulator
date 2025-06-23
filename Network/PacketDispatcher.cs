using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum;
using DRPGServer.Network.Handlers;

namespace DRPGServer.Network
{
    class PacketDispatcher
    {
        private readonly Dictionary<(SERVER_TYPE, ushort), IPacketHandler> dispatchers = [];

        public void Register(SERVER_TYPE serverType, ushort opcode, IPacketHandler handler)
        {
            var key = (serverType, opcode);
            if (!dispatchers.TryAdd(key, handler))
            {
                throw new InvalidOperationException($"Duplicate dispatcher for {serverType} / {opcode}");
            }
        }

        public bool Dispatch(InPacket packet, Client client)
        {
            var key = (client.ServerType, packet.PacketID);

            if (dispatchers.TryGetValue(key, out IPacketHandler? handler))
            {
                handler.Process(packet, client);
                return true;
            }

            return false;
        }
    }
}
