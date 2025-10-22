using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal class SendCommand : CommandState
    {
        private SendContext context;

        public SendCommand(string[] args, SendContext context) : base(args)
        {
            this.context = context;
        }

        private string Send()
        {
            return "";
        }

        public override string Run()
        {
            if(args.Length == 0)
            {
                return Send();    
            }

            switch(args[0])
            {
                case "-server":
                case "-s":
                    if(args.Length > 1)
                    {
                        context.SetServer(args[1]);
                        return new SendCommand(args.Skip(2).ToArray(),context).Run();
                    }
                    else
                    {
                        return $"No value for {args[0]} parameter provided";
                    }
                case "-message":
                case "-m":
                    if (args.Length > 1)
                    {
                        context.SetMessage(args[1]);
                        return new SendCommand(args.Skip(2).ToArray(),context).Run();
                    }
                    else
                    {
                        return $"No value for {args[0]} parameter provided";
                    }
                default:

                    return $"{args[0]} is not a valid parameter";
            }  
        }
    }
}
