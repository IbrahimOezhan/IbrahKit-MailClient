namespace MailClient.Commands
{
    internal interface ICommand
    {
        public static abstract string name { get; }
    }
}
