using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Data;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.Core.Network;
using TeamCaster.Core.Network.Pulse;
using TeamCaster.MVVM.Models;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class UserPoolViewModel : ViewModel
    {
        private Dictionary<UserModel, DateTime> _lastPulseTime;

        private object _usersThreadLock = new object();

        public  Pulse<UserModel> Pulse;

        public ObservableCollection<UserModel> Users { get; set; }

        public DataProvider<UserModel> PulseProvider;

        private Timer _userPoolHandler;

        public UserPoolViewModel(UserModel userInfo, INetworkDataSender networkDataSender)
        {
            Pulse = new Pulse<UserModel>(userInfo, networkDataSender);
            PulseProvider = new DataProvider<UserModel>();
            Users = new ObservableCollection<UserModel>();
            
            _lastPulseTime = new Dictionary<UserModel, DateTime>();

            PulseProvider.DataAvailable += OnPulseReceived;

            BindingOperations.EnableCollectionSynchronization(Users, _usersThreadLock);
        }

        public void StartPulse()
        {
            Thread newThread = new Thread(() => Pulse.Start());
            newThread.IsBackground = true;
            newThread.Start();

            _userPoolHandler = new Timer(HandleUserPool, null, 0, Pulse.Timeout);
        }

        private void OnPulseReceived(object sender, UserModel pulseObject)
        {
            if (!_lastPulseTime.ContainsKey(pulseObject))
            {
                Users.Add(pulseObject);
            }

            _lastPulseTime[pulseObject] = DateTime.Now;
        }

        private void HandleUserPool(object state)
        {
            DateTime currentTime = DateTime.Now;

            foreach (var item in _lastPulseTime)
            {
                if ((currentTime - item.Value).TotalMilliseconds > 2 * Pulse.Timeout)
                {
                    Users.Remove(item.Key);
                }
            }

        }

    }
}
