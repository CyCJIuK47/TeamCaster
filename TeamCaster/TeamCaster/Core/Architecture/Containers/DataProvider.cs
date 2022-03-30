using System;

namespace TeamCaster.Core.Architecture.Containers
{
    class DataProvider<T>
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
    }
}
