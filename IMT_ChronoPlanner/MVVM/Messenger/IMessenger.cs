using System;

namespace MVVM.Messenger
{
    public interface IMessenger
    {
        void Register<TNotification>(object recipient, Action<TNotification> action);

        void Register<TNotification>(object recipient, string identCode, Action<TNotification> action);

        void Send<TNotification>(TNotification notification);

        void Send<TNotification>(TNotification notification, string identCode);

        void Unregister<TNotification>(object recipient);
    }
}