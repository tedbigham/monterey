using System;
using System.Threading.Tasks;
using Twitch.Monterey.Web.WebSockets;

namespace Twitch.Monterey.Web.Contracts
{
    public abstract class MessageHandler
    {
        public abstract Task HandleMessage(object message, ClientSocket socket);
        public abstract Type GetMessageType();
    };

    public abstract class MessageHandler<T> : MessageHandler
    {
        public override Type GetMessageType()
        {
            return typeof(T);
        }
    }
}
