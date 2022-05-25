using System;
using System.Windows;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;
using TeamCaster.MVVM.Views.Windows;

namespace TeamCaster.MVVM.ViewModels
{
    class EntryPointViewModel : ViewModel
    {   
        public string Username { get; set; }

        public string IP { get; set; }
        
        public int Port { get; set; }

        public RelayCommand Connect { get; set; }

        public EntryPointViewModel()
        {
            Username = "User#1234";
            IP = "127.0.0.1";
            Port = 22222;
            
            Connect = new RelayCommand((o) =>
            {
                try
                {
                    TCPClient client = new TCPClient(IP, Port);
                    client.Connect();

                    MainWindow mainWindow = new MainWindow();
                    Application.Current.MainWindow = mainWindow;
                    
                    mainWindow.DataContext = new MainViewModel(Username, client);
                    mainWindow.Show();

                    UserEntry currentWindow = (UserEntry)o;
                    currentWindow.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ConnectionError", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                    
            });
        }
    }
}
