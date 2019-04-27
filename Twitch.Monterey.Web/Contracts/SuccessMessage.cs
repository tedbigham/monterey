using Twitch.Monterey.Web.Contracts;

namespace Twitch.Monterey.Web.WebSockets
{
    public class SuccessMessage : Message
    {
        public static readonly Message Instance = new SuccessMessage();

        public SuccessMessage()
        {
            Op = "success";
        }
    }
}
