using NAudio.Wave;

namespace TeamCaster.Core.Audio.IO.Codecs
{
    interface IAudioCodec
    {
        string Name { get; }

        bool IsAvailable { get; }

        int BitsPerSecond { get; }

        WaveFormat RecordFormat { get; }

        byte[] Encode(byte[] data, int offset, int length);

        byte[] Decode(byte[] data, int offset, int length);
    }
}
