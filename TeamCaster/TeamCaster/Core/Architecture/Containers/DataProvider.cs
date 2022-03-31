using System;
using TeamCaster.Core.Architecture.Observer;

namespace TeamCaster.Core.Architecture.Containers
{
    class DataProvider<T> : ISubscriber
    {
        private T _data;

        public Type DataType { get; private set; }

        public T Data
        {
            get => _data;
            set
            {
                _data = value;
                DataAvailable.Invoke(this, value);
            }
        }

        public DataProvider()
        {
            DataType = typeof(T);
        }

        public event Action<object, T> DataAvailable;

        public void Update(dynamic context)
        {
            Data = (T)context;
        }
    }
}
