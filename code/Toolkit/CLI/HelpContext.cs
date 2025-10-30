namespace MailClient.code.Toolkit.CLI
{
    internal class HelpContext : Context
    {
        private string commandName = string.Empty;

        public void SetCommandName(string value) { commandName = value; }

        public string GetCommandName() => commandName;
    }
}
