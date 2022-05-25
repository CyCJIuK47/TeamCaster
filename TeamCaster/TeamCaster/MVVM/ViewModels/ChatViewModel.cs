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
        public DataProvider<MessageModel> _messageProvider = new DataProvider<MessageModel>();
        private ObservableCollection<MessageModel> _messages = new ObservableCollection<MessageModel>();

        private object _messagesThreadLock = new object();

        public string CurrentMessage
        {
            get => _currentMessage;
            set => Set(ref _currentMessage, value);
        }

        public ObservableCollection<MessageModel> Messages { get => _messages; set => Set(ref _messages, value); }

        public DataProvider<MessageModel> MessageProvider => _messageProvider;

        public ChatViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(_messages, _messagesThreadLock);

            _messageProvider.DataAvailable += OnNewMessageAvailable;
        }

        private void OnNewMessageAvailable(object sender, MessageModel newMessage)
        {
            _messages.Add(newMessage);
        }

    }
}
