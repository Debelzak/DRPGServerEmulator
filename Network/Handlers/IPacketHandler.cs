using DRPGServer.Network.Packets;
using DRPGServer.Network.Enum;

namespace DRPGServer.Network.Handlers
{
    interface IPacketHandler
    {
        public ushort Opcode { get; }
        public SERVER_TYPE ServerType { get; }
        
        void Process(InPacket packet, Client client);
    }
}