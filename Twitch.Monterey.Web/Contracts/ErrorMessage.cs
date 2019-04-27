using Twitch.Monterey.Web.Contracts;

namespace Twitch.Monterey.Web.WebSockets
{
    public class ErrorMessage : Message
    {
        public string Message { get; set; }

        public ErrorMessage()
        {
            Op = "error";
        }
    }
}
