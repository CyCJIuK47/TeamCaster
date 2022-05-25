using System;
using System.Collections.ObjectModel;
using System.Threading;
using TeamCaster.Core.Architecture;
using TeamCaster.Core.Architecture.Observer;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;
using TeamCaster.MVVM.Models;
using TeamCaster.MVVM.Views.Windows;


namespace TeamCaster.MVVM.ViewModels
{
    class MainViewModel : ViewModel
    {
        public NetworkViewModel NetworkViewModel { get; }

        public UserPoolViewModel UserPoolViewModel { get; }

        public ChatViewModel ChatViewModel { get; set; }

        private DataPublisher _dataPublisher = new DataPublisher();

        public DataPublisher DataPublisher => _dataPublisher;

        public AudioSystemViewModel AudioSystemViewModel { get; set; }

        public ScreenSharingViewModel ScreenSharingViewModel { get; set; }

        public InteractivityViewModel InteractivityViewModel { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }

        public RelayCommand SendMessage { get; set; }

        public RelayCommand OpenScreenViewWindow { get; set; }

        public UserInfoViewModel UserInfoViewModel { get; set; }

        public MainViewModel(string username, TCPClient client)
        {
            NetworkViewModel = new NetworkViewModel(client);
            UserInfoViewModel = new UserInfoViewModel(username);
            ChatViewModel = new ChatViewModel();

            AudioSystemViewModel = new AudioSystemViewModel(NetworkViewModel.Client);
            InteractivityViewModel = new InteractivityViewModel(NetworkViewModel.Client);
            ScreenSharingViewModel = new ScreenSharingViewModel(NetworkViewModel.Client);

            UserPoolViewModel = new UserPoolViewModel(new UserModel {Username = UserInfoViewModel.Username },
                                                                     NetworkViewModel.Client);
            
            DataPublisher.AddSubscriber(AudioSystemViewModel.AudioPlayerViewModel.DataProvider.DataType,
                                        AudioSystemViewModel.AudioPlayerViewModel.DataProvider);

            DataPublisher.AddSubscriber(ScreenSharingViewModel.ScreenViewer.DataProvider.DataType,
                                        ScreenSharingViewModel.ScreenViewer.DataProvider);

            DataPublisher.AddSubscriber(ScreenSharingViewModel.ScreenViewer.ScreenOptionsProvider.DataType,
                                        ScreenSharingViewModel.ScreenViewer.ScreenOptionsProvider);

            DataPublisher.AddSubscriber(InteractivityViewModel.MouseSimulatorViewModel.MouseEventProvider.DataType,
                                        InteractivityViewModel.MouseSimulatorViewModel.MouseEventProvider);


            DataPublisher.AddSubscriber(UserPoolViewModel.PulseProvider.DataType,
                                        UserPoolViewModel.PulseProvider);

            SendMessage = new RelayCommand(
                (object p) =>
                {
                    MessageModel message = new MessageModel{
                        Message = ChatViewModel.CurrentMessage,
                        Username = UserInfoViewModel.Username,
                        Time = DateTime.Now.ToString("HH:mm")
                    };

                    ChatViewModel.Messages.Add(message);
                    NetworkViewModel.Send(message);

                    ChatViewModel.CurrentMessage = string.Empty;
                },
                (object p) => ChatViewModel.CurrentMessage != string.Empty);

            var receivedDataHandler = new ReceivedDataHandler(NetworkViewModel.Client, _dataPublisher);

            _dataPublisher.AddSubscriber(ChatViewModel.MessageProvider.DataType, ChatViewModel.MessageProvider);

            
            Thread newThread = new Thread(() => receivedDataHandler.Start());
            newThread.IsBackground = true;
            newThread.Start();

            UserPoolViewModel.StartPulse();
            
            OpenScreenViewWindow = new RelayCommand(
                (object p) =>
                {
                    ScreenSharingWindow screenSharingWindow = new ScreenSharingWindow();
                    screenSharingWindow.DataContext = this;
                    
                    screenSharingWindow.Show();
                });

        }
    }
}
