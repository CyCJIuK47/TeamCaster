using System;

namespace TeamCaster.MVVM.Models
{   
    [Serializable]
    class MessageModel
    {
        public string Username { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }
    }
}
