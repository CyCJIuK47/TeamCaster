using System;
using TeamCaster.Core.Audio.IO;
using TeamCaster.Core.Audio.IO.Codecs;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class AudioSystemViewModel : ViewModel
    {
        public AudioPlayerViewModel AudioPlayerViewModel { get; set; }

        public MicrophoneRecorderViewModel MicrophoneRecorderViewModel { get; set; }

        public AudioSystemViewModel(INetworkDataSender networkDataSender)
        {
            AudioPlayerViewModel = new AudioPlayerViewModel();
            MicrophoneRecorderViewModel = new MicrophoneRecorderViewModel(new NetworkAudioSender(networkDataSender,
                                                                          new G722AudioCodec()));
        }
    }
}
