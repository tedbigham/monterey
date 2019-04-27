namespace Twitch.Monterey.Web.Contracts
{
    public class ChatMessage : RoomMessage
    {
        public string Message { get; set; }
        public string Sender { get; set; }

        public ChatMessage()
        {
            Op = "message";
        }
    }
}
