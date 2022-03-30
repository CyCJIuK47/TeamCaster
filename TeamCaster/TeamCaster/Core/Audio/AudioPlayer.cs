using System;
using System.IO;
using NAudio;
using NAudio.Wave;
using TeamCaster.Core.Architecture.Containers;

namespace TeamCaster.Core.Audio
{
    class AudioPlayer
    {
        DataProvider<byte[]> _dataProvider;
        private NAudio.Wave.DirectSoundOut _outStream;

        public AudioPlayer(DataProvider<byte[]> dataProvider)
        {
            _dataProvider = dataProvider;
            _dataProvider.DataAvailable += onDataAvailable;

            _outStream = new NAudio.Wave.DirectSoundOut();
        }

        private void onDataAvailable(object sender, byte[] data)
        {
            IWaveProvider provider = new RawSourceWaveStream(
                         new MemoryStream(data), new WaveFormat());

            _outStream.Init(provider);
            _outStream.Play();
        }
    }
}
