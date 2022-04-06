using System;
using System.Windows.Data;
using System.Collections.ObjectModel;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.MVVM.Models;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class ChatViewModel : ViewModel
    {

        private string _currentMessage = string.Empty;

        public string CurrentMessage
        {
            get => _currentMessage;
            set => Set(ref _currentMessage, value);
        }

        private object _messagesThreadLock = new object();

        private ObservableCollection<MessageModel> _messages = new ObservableCollection<MessageModel>();

        public ObservableCollection<MessageModel> Messages { get => _messages; set => Set(ref _messages, value); }

        public DataProvider<MessageModel> _messageProvider = new DataProvider<MessageModel>();

        public DataProvider<MessageModel> MessageProvider => _messageProvider;

        private void OnNewMessageAvailable(object sender, MessageModel newMessage)
        {
            _messages.Add(newMessage);
        }

        public ChatViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(_messages, _messagesThreadLock);

            Messages.Add(new MessageModel
            {
                Username = "TestUsername",
                Message = "Hello from here",
                Time = DateTime.Now.ToString("hh:MM")
            });

            _messageProvider.DataAvailable += OnNewMessageAvailable;
        }

    }
}
