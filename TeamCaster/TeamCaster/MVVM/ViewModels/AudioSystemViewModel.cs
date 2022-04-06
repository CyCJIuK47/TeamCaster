using System;
using TeamCaster.Core.Audio;
using TeamCaster.Core.Audio.IO;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.ViewModels.Base;
using TeamCaster.MVVM.Commands;

namespace TeamCaster.MVVM.ViewModels
{
    class AudioSystemViewModel : ViewModel
    {
        public AudioPlayerViewModel AudioPlayerViewModel { get; set; }

        public MicrophoneRecorderViewModel MicrophoneRecorderViewModel { get; set; }

        public AudioSystemViewModel(INetworkDataSender networkDataSender)
        {
            AudioPlayerViewModel = new AudioPlayerViewModel();
            MicrophoneRecorderViewModel = new MicrophoneRecorderViewModel(new NetworkAudioSender(networkDataSender));
        }
    }
}
