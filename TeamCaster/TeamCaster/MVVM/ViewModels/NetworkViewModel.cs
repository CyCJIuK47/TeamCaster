using System;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class NetworkViewModel : ViewModel
    {
        private TCPClient _client;

        public TCPClient Client { get => _client; }

        public RelayCommand Connect { get; private set; }

        public void Send<T>(T data)
        {
            _client.Send(data);
        }

        public NetworkViewModel(TCPClient client)
        {
            _client = client;
            
            Connect = new RelayCommand((object p) => _client.Connect(), (object p) => true);
        }

    }
}
