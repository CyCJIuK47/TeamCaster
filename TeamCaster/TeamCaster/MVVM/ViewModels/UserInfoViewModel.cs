using System;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class UserInfoViewModel : ViewModel
    {
        public string Username { get; set; }

        public UserInfoViewModel(string username)
        {
            Username = username;
        }
    }
}
