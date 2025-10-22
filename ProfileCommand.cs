using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient
{
    internal class ProfileCommand : CommandState
    {
        private ProfileContext context;

        public ProfileCommand(string[] args, ProfileContext context) : base(args)
        {
            this.context = context;
        }

        private void Execute()
        {
            switch (context.GetMode())
            {
                case ProfileContext.Mode.LIST:
                    break;
                case ProfileContext.Mode.CREATE:
                    break;
                case ProfileContext.Mode.DELETE:
                    break;
            }
        }

        public override string Run()
        {
            if(args.Length == 0)
            {
                Execute();
            }

            switch(args[0])
            {
                case "-n":
                case "-name":

                    if (args.Length == 1)
                    {
                        return $"No value provided for {args[0]}";
                    }

                    context.SetName(args[1]);

                    return new ProfileCommand(args.Skip(2).ToArray(), context).Run();

                case "-m":
                case "-mode":

                    if(args.Length == 1)
                    {
                        return $"No value provided for {args[0]}";
                    }

                    if(Enum.TryParse<ProfileContext.Mode>(args[1], out ProfileContext.Mode result))
                    {
                        context.SetMode(result);

                        return new ProfileCommand(args.Skip(2).ToArray(), context).Run();
                    }
                    else
                    {
                        return $"{args[0]} is not a valid profile mode";
                    }
                default:
                    return $"{args[0]} is an invalid argument";
            }
        }
    }
}
