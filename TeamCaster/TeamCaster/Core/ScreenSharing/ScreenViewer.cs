using System;
using System.Drawing;
using System.IO;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.Core.ScreenSharing.IO;

namespace TeamCaster.Core.ScreenSharing
{
    class ScreenViewer
    {
        private int _width;
        private int _height;

        private Bitmap _screen;
        private Graphics _graphics;

        public DataProvider<ScreenFrame> DataProvider;

        public DataProvider<ScreenSharingOptions> ScreenOptionsProvider;

        public int TargetWidth { get; set; }

        public int TargetHeight { get; set; }

        public Action<object, ScreenFrame> SceneUpdated;

        public Bitmap Source => _screen;

        public ScreenViewer()
        {
            DataProvider = new DataProvider<ScreenFrame>();
            DataProvider.DataAvailable += OnDataAvailable;

            ScreenOptionsProvider = new DataProvider<ScreenSharingOptions>();
            ScreenOptionsProvider.DataAvailable += OnScreenSharingOptionsReceived;
        }

        private void OnDataAvailable(object sender, ScreenFrame screenFrame)
        {
            var ms = new MemoryStream(screenFrame.BitmapArray);
            var bmp = new Bitmap(ms);

            _graphics = Graphics.FromImage(_screen);
            _graphics.DrawImage(bmp, screenFrame.PosX, screenFrame.PosY);
            
            SceneUpdated?.Invoke(this, screenFrame);
        }

        private void OnScreenSharingOptionsReceived(object sender, ScreenSharingOptions screenSharingOptions)
        {
            _width = screenSharingOptions.Width;
            _height = screenSharingOptions.Height;

            _screen = new Bitmap(_width, _height);

        }

    }
}
