using System;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.Core.Audio;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class AudioPlayerViewModel : ViewModel
    {
        private AudioPlayer _audioPlayer;

        public DataProvider<byte[]> DataProvider = new DataProvider<byte[]>();

        private string[] _imagesSources;

        private string _imageSource;

        public string ImageSource { 
            get => _imageSource; 
            set => Set(ref _imageSource, value);
        }

        public RelayCommand ToggleMuteState { get; set; }

        public AudioPlayerViewModel()
        {
            _audioPlayer = new AudioPlayer(DataProvider);
            ToggleMuteState = new RelayCommand((object p) => {
                _audioPlayer.Muted = !_audioPlayer.Muted;
                ImageSource = _imagesSources[Convert.ToInt32(_audioPlayer.Muted)];
                OnPropertyChanged("ImageSource");
            });

            _imagesSources = new string[] { "./../../../Icons/headphone-on.png",
                                           "./../../../Icons/headphone-off.png"};

            ImageSource = _imagesSources[Convert.ToInt32(_audioPlayer.Muted)];
        }
    }
}
