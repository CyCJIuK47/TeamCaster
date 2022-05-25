using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TeamCaster.Core.Network;
using TeamCaster.Core.ScreenSharing.IO;

namespace TeamCaster.Core.ScreenSharing
{
    class ScreenRecorder
    {
        private INetworkDataSender _networkDataSender;

        private int _width;
        private int _height;

        private int _frameWidth;
        private int _frameHeight;

        private int _frameXCount;
        private int _frameYCount;

        private int _cornerWidth;
        private int _cornerHeight;

        private Bitmap[] _previousFrames;
        private Bitmap[] _currentFrames;

        private Bitmap _currentScreen;

        public int Width => _width;
        public int Height => _height;
        public bool RecordingState { get; set; }

        public int FPS { get; set; }

        public ScreenRecorder(int frameWidth, int frameHeight, INetworkDataSender networkDataSender, int fps = 1)
        {
            _width = Screen.PrimaryScreen.Bounds.Width;
            _height = Screen.PrimaryScreen.Bounds.Height;

            _currentScreen = new Bitmap(_width, _height);

            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _networkDataSender = networkDataSender;

            _frameXCount = (_width + _frameWidth - 1) / _frameWidth;
            _frameYCount = (_height + _frameHeight - 1) / _frameHeight;

            _cornerWidth = (_width % _frameWidth) == 0 ? _frameWidth : (_width % _frameWidth);
            _cornerHeight = (_height % _frameHeight) == 0 ? _frameHeight : (_height % _frameHeight);

            _previousFrames = new Bitmap[_frameXCount * _frameYCount];


            InitFrames(ref _currentFrames);
            InitFrames(ref _previousFrames);

            RecordingState = false;
            FPS = fps;
        }

        public void StartRecording()
        {
            RecordingState = true;
            _networkDataSender.Send(new ScreenSharingOptions(_width, _height));

            while (RecordingState)
            {
                CaptureScreen();
                CropScreen();
                Thread.Sleep(1000 / FPS);
            }
        }

        public void StopRecording()
        {
            RecordingState = false;
        }

        private void InitFrames(ref Bitmap[] frames)
        {
            frames = new Bitmap[_frameXCount * _frameYCount];

            for (int i = 0; i < _frameYCount; ++i)
            {
                for (int j = 0; j < _frameXCount; ++j)
                {
                    int height = (i == _frameYCount - 1) ? _cornerHeight : _frameHeight;
                    int width = (j == _frameXCount - 1) ? _cornerWidth : _frameWidth;

                    frames[_frameXCount * i + j] = new Bitmap(width, height);
                }
            }
        }

        private void CaptureScreen()
        {
            Graphics graphics = Graphics.FromImage(_currentScreen);
            graphics.CopyFromScreen(0, 0, 0, 0, _currentScreen.Size);
        }

        private void CropScreen()
        {
            for (int i = 0; i < _frameYCount; ++i)
            {
                for (int j = 0; j < _frameXCount; ++j)
                {
                    int currentFrameId = _frameXCount * i + j;

                    int xpos = _frameWidth * j;
                    int ypos = _frameHeight * i;

                    _currentFrames[currentFrameId] = _currentScreen.Clone(new Rectangle(xpos, ypos,
                                                                          _currentFrames[currentFrameId].Width,
                                                                          _currentFrames[currentFrameId].Height),
                                                                          _currentScreen.PixelFormat);

                    bool isEqual = CompareFrames(_previousFrames[currentFrameId], _currentFrames[currentFrameId]);

                    if (!isEqual)
                    {
                        SendFrame(xpos, ypos, _currentFrames[currentFrameId]);
                        SwapFrames(currentFrameId);
                    }
                }
            }
        }

        private void SendFrame(int x, int y, Bitmap frame)
        {
            using (System.IO.MemoryStream sampleStream = new System.IO.MemoryStream())
            {
                frame.Save(sampleStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                _networkDataSender.Send(new ScreenFrame(x, y, sampleStream.ToArray()));
            }
        }

        private void SwapFrames(int position)
        {
            Bitmap tmp = _currentFrames[position];
            _currentFrames[position] = _previousFrames[position];
            _previousFrames[position] = tmp;
        }

        private bool CompareFrames(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
            if (!bmp1.Size.Equals(bmp2.Size) || !bmp1.PixelFormat.Equals(bmp2.PixelFormat))
                return false;

            int bytes = bmp1.Width * bmp1.Height * (Image.GetPixelFormatSize(bmp1.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bitmapData1 = bmp1.LockBits(new Rectangle(0, 0, bmp1.Width, bmp1.Height), ImageLockMode.ReadOnly, bmp1.PixelFormat);
            BitmapData bitmapData2 = bmp2.LockBits(new Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadOnly, bmp2.PixelFormat);

            Marshal.Copy(bitmapData1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bitmapData2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n < bytes; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            bmp1.UnlockBits(bitmapData1);
            bmp2.UnlockBits(bitmapData2);

            return result;
        }

    }
}
