using TeamCaster.Core.Network;
using TeamCaster.Core.Audio.IO.Codecs;

namespace TeamCaster.Core.Audio.IO
{
    class NetworkAudioSender : IAudioSender
    {
        private INetworkDataSender _networkDataSender;
        private IAudioCodec _audioCodec;

        public NetworkAudioSender(INetworkDataSender networkDataSender, IAudioCodec audioCodec)
        {
            _networkDataSender = networkDataSender;
            _audioCodec = audioCodec;
        }

        public void Send(byte[] data)
        {   
            _networkDataSender.Send(_audioCodec.Encode(data, 0, data.Length));
        }
    }
}
