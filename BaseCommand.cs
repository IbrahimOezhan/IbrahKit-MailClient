using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal class BaseCommand : CommandState
    {
        public BaseCommand(string[] args) : base(args)
        {
        }

        public override string Run()
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No command provided. Possible commands are: send, profile");
                return null;
            }

            switch(args[0].ToLower())
            {
                case "send":
                    return new SendCommand(args.Skip(1).ToArray());
                case "profile":
                    return new ProfileCommand(args.Skip(1).ToArray());
                default:
                    Console.WriteLine($"Unknown command: {args[0]}. Possible commands are: send, profile");
                    return null;
            }
        }
    }
}
