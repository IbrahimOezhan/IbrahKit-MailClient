using MailClient.Configs;
using MailClient.History;
using MailClient.Utilities;
using System.Net;
using System.Net.Mail;

namespace MailClient
{
    internal class MailClient
    {
        public const string FOLDER = "IbrahKit";

        private Dictionary<string, Action<string[]>> commands = new();

        public string Run(string[] args)
        {
            commands.Add("send", (arguments) =>
            {
                Send(arguments);
            });

            commands.Add("profile", (arguments) =>
            {
                Profile(arguments);
            });

            try
            {
                switch(args.Length)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                }


                return Send(args);
            }
            catch
            {
                throw;
            }
        }

        private static string Profile(string[] args)
        {
            if(args.Length == 0)
            {
                return "profile command requires arguments. Possible are:";
            }
            else
            {

            }

            return "";
        }

        private static string Send(string[] args)
        {

        }
    }
}
