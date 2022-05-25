using System;
using System.IO;
using NAudio.Wave;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.Core.Audio.IO.Codecs;

namespace TeamCaster.Core.Audio
{
    class AudioPlayer
    {
        DataProvider<byte[]> _dataProvider;
        private DirectSoundOut _outStream;
        private IAudioCodec _audioCodec;

        public bool Muted { get; set; }

        public AudioPlayer(DataProvider<byte[]> dataProvider, IAudioCodec audioCodec)
        {
            _dataProvider = dataProvider;
            _audioCodec = audioCodec;
            _dataProvider.DataAvailable += onDataAvailable;

            _outStream = new DirectSoundOut();
            Muted = false;
        }

        private void onDataAvailable(object sender, byte[] data)
        {
            if (Muted) return;

            IWaveProvider waveProvider = new RawSourceWaveStream(new MemoryStream(_audioCodec.Decode(data, 0, data.Length)),
                                                                 new WaveFormat());

            _outStream.Init(waveProvider);
            _outStream.Play();
        }
    }
}
