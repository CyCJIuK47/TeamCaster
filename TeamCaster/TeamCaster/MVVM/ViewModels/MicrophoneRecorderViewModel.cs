using System;
using System.Threading;
using TeamCaster.Core.Audio;
using TeamCaster.Core.Audio.IO;
using TeamCaster.Core.Audio.IO.Codecs;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class MicrophoneRecorderViewModel : ViewModel
    {
        private MicrophoneRecorder _microphoneRecorder;

        public RelayCommand ToggleRecording { get; set; }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        private string[] _imagesSources;

        public MicrophoneRecorderViewModel(IAudioSender audioSender)
        {
            _microphoneRecorder = new MicrophoneRecorder(audioSender);
            _imagesSources = new string[] { "./../../../Icons/microphone-off.png",
                                           "./../../../Icons/microphone-on.png"};

            ImageSource = _imagesSources[Convert.ToInt32(_microphoneRecorder.RecordingState)];

            ToggleRecording = new RelayCommand((object p) => 
            {
                ImageSource = _imagesSources[Convert.ToInt32(!_microphoneRecorder.RecordingState)];

                if (_microphoneRecorder.RecordingState == false)
                {
                    Thread newThread = new Thread(() => _microphoneRecorder.StartRecording());
                    newThread.IsBackground = true;
                    newThread.Start();
                }
                else
                {
                    _microphoneRecorder.StopRecording();
                }
            });
        }
    }
}
