namespace BookStore.Infra.Messaging
{
    public class Mail
    {
        public string Server { get; set; }
        public int Port { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
