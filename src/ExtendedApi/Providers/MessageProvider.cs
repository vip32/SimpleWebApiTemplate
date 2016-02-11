namespace ExtendedApi
{
    public class MessageProvider : IMessageProvider
    {
        public string WelcomeText
        {
            get { return "Welcome to the Extended WebApi (MessageProvider)"; }
        }
    }
}