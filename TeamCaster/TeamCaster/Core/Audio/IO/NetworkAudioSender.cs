using TeamCaster.Core.Network;

namespace TeamCaster.Core.Audio.IO
{
    class NetworkAudioSender : IAudioSender
    {
        private INetworkDataSender _networkDataSender;
        public NetworkAudioSender(INetworkDataSender networkDataSender)
        {
            _networkDataSender = networkDataSender;
        }

        public void Send(byte[] data)
        {
            _networkDataSender.Send(data);
        }
    }
}
