using System;
using System.Windows.Input;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.ViewModels.Base;
using TeamCaster.MVVM.Commands;

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

        public NetworkViewModel()
        {
            _client = new TCPClient("127.0.0.1", 22222);
            
            Connect = new RelayCommand((object p) => _client.Connect(), (object p) => true);

            Connect.Execute(null);
        }

    }
}
