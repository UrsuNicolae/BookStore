namespace BookStore.Infra.Messaging
{
    public class MessageOptions
    {

        public MessageOptions()
        {
            fromDisplayName = "BookStore";
            fromEmailAddress = "BookStore@gmail.com";
        }
        public string fromDisplayName { get; set; }

        public string fromEmailAddress { get; set; }

        public string toEamilAddress { get; set; }

        public string subjcet { get; set; }

        public string message { get; set; }
    }
}
