using System;

namespace TeamCaster.Core.ScreenSharing.IO
{   
    [Serializable]
    class ScreenFrame
    {
        public int PosX;
        public int PosY;
        public byte[] BitmapArray;

        public ScreenFrame(int posX, int posY, byte[] bitmap)
        {
            PosX = posX;
            PosY = posY;
            BitmapArray = bitmap;
        }
    }
}
