using System;
using NAudio.Wave;
using TeamCaster.Core.Audio.IO;

namespace TeamCaster.Core.Audio
{
    class MicrophoneRecorder
    {
        private NAudio.Wave.WaveInEvent _sourceStream;
        private IAudioSender _audioSender;

        public MicrophoneRecorder(IAudioSender audioSender)
        {
            _sourceStream = null;
            _audioSender = audioSender;
        }

        public void StartRecording()
        {
            _sourceStream = new NAudio.Wave.WaveInEvent();
            _sourceStream.BufferMilliseconds = 40;
            _sourceStream.DeviceNumber = 0;
            _sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(44100, 2);

            _sourceStream.DataAvailable += waveIn_DataAvailable;

            _sourceStream.StartRecording();
        }

        public void StopRecording()
        {
            _sourceStream.StopRecording();
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            _audioSender.Send(e.Buffer);
        }
    }
}
