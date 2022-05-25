using System;

namespace TeamCaster.Core.ScreenSharing.IO
{
    [Serializable]
    class ScreenSharingOptions
    {
        public int Width;

        public int Height;

        public ScreenSharingOptions(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
