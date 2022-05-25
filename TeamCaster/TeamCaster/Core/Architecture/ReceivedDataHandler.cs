using System;
using System.Threading.Tasks;
using TeamCaster.Core.Architecture.Observer;
using TeamCaster.Core.Network;

namespace TeamCaster.Core.Architecture
{
    class ReceivedDataHandler
    {
        private INetworkDataReceiver _networkDataReceiver;
        private DataPublisher _dataPublisher;
        private bool _isRunning;

        public ReceivedDataHandler(INetworkDataReceiver networkDataReceiver, DataPublisher dataPublisher)
        {
            _networkDataReceiver = networkDataReceiver;
            _dataPublisher = dataPublisher;
        }

        public void Start()
        {
            _isRunning = true;

            while (_isRunning)
            {
                var data = _networkDataReceiver.Receive();
                //Task.Run(() => { _dataPublisher.Publish(data); });
                _dataPublisher.Publish(data);
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}
