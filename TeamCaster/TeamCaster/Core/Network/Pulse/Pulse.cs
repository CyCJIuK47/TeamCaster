using System;
using System.Threading;

namespace TeamCaster.Core.Network.Pulse
{
    class Pulse<T>
    {
        private T _pulseObject;
        private INetworkDataSender _networkDataSender;

        private bool _isRunning;

        public int Timeout { get; set; }

        public Pulse(T pulseObject, INetworkDataSender networkDataSender, int millisecondsTimeOut=2000)
        {
            _pulseObject = pulseObject;
            _networkDataSender = networkDataSender;
            
            Timeout = millisecondsTimeOut;

            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;

            while(_isRunning)
            {
                _networkDataSender.Send(_pulseObject);
                Thread.Sleep(Timeout);
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

    }
}
