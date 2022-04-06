using System;
using System.Collections.ObjectModel;
using System.Threading;
using TeamCaster.Core.Architecture;
using TeamCaster.Core.Architecture.Observer;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;
using TeamCaster.MVVM.Models;


namespace TeamCaster.MVVM.ViewModels
{
    class MainViewModel : ViewModel
    {
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();

        public UserPoolViewModel UserPoolViewModel { get; } = new UserPoolViewModel();

        public ChatViewModel ChatViewModel { get; set; } = new ChatViewModel();

        private DataPublisher _dataPublisher = new DataPublisher();

        public DataPublisher DataPublisher  => _dataPublisher;
        
        public AudioSystemViewModel AudioSystemViewModel { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }

        public RelayCommand SendMessage { get; set; }


        public MainViewModel()
        {
            AudioSystemViewModel = new AudioSystemViewModel(NetworkViewModel.Client);
            DataPublisher.AddSubscriber(AudioSystemViewModel.AudioPlayerViewModel.DataProvider.DataType,
                                        AudioSystemViewModel.AudioPlayerViewModel.DataProvider);

            SendMessage = new RelayCommand(
                (object p) => {
                    ChatViewModel.Messages.Add(
                        new MessageModel
                        {
                            Message = ChatViewModel.CurrentMessage,
                            Username = "Tester",
                            Time = DateTime.Now.ToString("hh:MM")
                        }
                    );
                    NetworkViewModel.Send(
                        new MessageModel
                        {
                            Message = ChatViewModel.CurrentMessage,
                            Username = "Tester",
                            Time = DateTime.Now.ToString("hh:MM")
                        });
                    ChatViewModel.CurrentMessage = string.Empty;
                },
                (object p) => ChatViewModel.CurrentMessage != string.Empty);

            var receivedDataHandler = new ReceivedDataHandler(NetworkViewModel.Client, _dataPublisher);

            _dataPublisher.AddSubscriber(ChatViewModel.MessageProvider.DataType, ChatViewModel.MessageProvider);

            
            Thread newThread = new Thread(() => receivedDataHandler.Start());
            newThread.IsBackground = true;
            newThread.Start();

        }
    }
}
