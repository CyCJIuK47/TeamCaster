using System;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TeamCaster.MVVM.Commands;
using TeamCaster.Core.Network;
using TeamCaster.Core.ScreenSharing;
using TeamCaster.Core.ScreenSharing.IO;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class ScreenSharingViewModel : ViewModel
    {
        public ScreenRecorder ScreenRecorder { get; set; }

        public ScreenViewer ScreenViewer { get; set; }

        private BitmapImage _source;

        public BitmapImage Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        private string[] _imagesSources;

        private string _imageSource;

        public string ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        public RelayCommand ToggleRecording { get; set; }

        private DispatcherTimer _sceneUpdateHandler;

        public ScreenSharingViewModel(INetworkDataSender networkDataSender)
        {   
            ScreenRecorder = new ScreenRecorder(256, 256, networkDataSender);
            ScreenViewer = new ScreenViewer();

            ScreenViewer.SceneUpdated += OnSceneUpdated;

            Source = BmpImageFromBmp(new Bitmap(1, 1));
             
            _imagesSources = new string[] { "./../../../Icons/start-sharing.png",
                                            "./../../../Icons/stop-sharing.png"};

            ImageSource = _imagesSources[Convert.ToInt32(ScreenRecorder.RecordingState)];

            ToggleRecording = new RelayCommand((object p) =>
            {
                ImageSource = _imagesSources[Convert.ToInt32(!ScreenRecorder.RecordingState)];

                if (ScreenRecorder.RecordingState == false)
                {
                    Thread newThread = new Thread(() => ScreenRecorder.StartRecording());
                    newThread.IsBackground = true;
                    newThread.Start();
                }
                else
                {
                    ScreenRecorder.StopRecording();
                }
            });

            _sceneUpdateHandler = new DispatcherTimer();
            _sceneUpdateHandler.Tick += new EventHandler(dispatcherTimer_Tick);
            _sceneUpdateHandler.Interval = new TimeSpan(0, 0, 1);
            _sceneUpdateHandler.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (ScreenViewer.Source!=null)
                Source = BmpImageFromBmp(ScreenViewer.Source);                

            OnPropertyChanged("Source");
        }

        private void OnSceneUpdated(object sender, ScreenFrame screeFrame)
        {
        }

        private BitmapImage BmpImageFromBmp(Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

    }
}
