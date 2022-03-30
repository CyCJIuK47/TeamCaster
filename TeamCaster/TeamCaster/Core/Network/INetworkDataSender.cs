
namespace TeamCaster.Core.Network
{
    interface INetworkDataSender
    {
        void Send<T>(T data);
    }
}
