using System;
using NAudio.Wave;

namespace TeamCaster.Core.Audio.IO.Codecs
{
    class NoCompressionCodec : IAudioCodec
    {
        public NoCompressionCodec()
        {
            RecordFormat = new WaveFormat(8000, 16, 1); ;
        }

        public string Name => "NoCompressionCodec";

        public bool IsAvailable => true;

        public int BitsPerSecond => RecordFormat.AverageBytesPerSecond * 8;

        public WaveFormat RecordFormat { get; private set; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            var encoded = new byte[length];
            Array.Copy(data, offset, encoded, 0, length);
            return encoded;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            var decoded = new byte[length];
            Array.Copy(data, offset, decoded, 0, length);
            return decoded;
        }
    }
}
