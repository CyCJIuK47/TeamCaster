using System;
using NAudio.Wave;
using NAudio.Codecs;
using System.Diagnostics;

namespace TeamCaster.Core.Audio.IO.Codecs
{
    class G722AudioCodec : IAudioCodec
    {
        private readonly int _bitrate;
        private readonly G722CodecState _encoderState;
        private readonly G722CodecState _decoderState;
        private readonly G722Codec _codec;

        public G722AudioCodec()
        {
            _bitrate = 64000;
            _encoderState = new G722CodecState(_bitrate, G722Flags.None);
            _decoderState = new G722CodecState(_bitrate, G722Flags.None);
            _codec = new G722Codec();
            RecordFormat = new WaveFormat(16000, 1);
        }

        public string Name => "G.722 16kHz";

        public int BitsPerSecond => _bitrate;

        public WaveFormat RecordFormat { get; }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            var wb = new WaveBuffer(data);
            int encodedLength = length / 4;
            var outputBuffer = new byte[encodedLength];
            _codec.Encode(_encoderState, outputBuffer, wb.ShortBuffer, length / 2);
            
            return outputBuffer;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            int decodedLength = length * 4;
            var outputBuffer = new byte[decodedLength];
            var wb = new WaveBuffer(outputBuffer);
            _codec.Decode(_decoderState, wb.ShortBuffer, data, length);

            return outputBuffer;
        }

        public bool IsAvailable => true;
    }
}
