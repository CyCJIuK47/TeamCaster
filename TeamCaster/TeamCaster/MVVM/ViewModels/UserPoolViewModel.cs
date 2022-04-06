using System;
using System.Collections.ObjectModel;
using TeamCaster.MVVM.Models;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class UserPoolViewModel : ViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public UserPoolViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            
            Users.Add(new UserModel
            {
                Username = "Username1"
            });

            Users.Add(new UserModel
            {
                Username = "Username2"
            });
        }
    }
}
