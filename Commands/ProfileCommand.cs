using MailClient.Configs;
using System.Text;

namespace MailClient.Commands
{
    internal class ProfileCommand : Command
    {
        private ProfileContext context;

        public ProfileCommand(string[] args) : base(args)
        {
            this.context = new();
        }

        public ProfileCommand(string[] args, ProfileContext context) : base(args)
        {
            this.context = context;
        }

        public override string Execute()
        {
            StringBuilder sb = new();

            switch (context.GetMode())
            {
                case ProfileContext.Mode.LIST:

                    sb.AppendLine("Found the following profiles");

                    foreach (var profile in ProfileConfig.GetConfigs().Where(x => x.GetProfileName().Contains(context.GetProfile(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sb.AppendLine(profile.ToString());
                    }

                    break;
                case ProfileContext.Mode.CREATE:

                    bool result = ProfileConfig.TryCreate(context.GetProfile(), out _);

                    sb.AppendLine($"{context.GetProfile()} profile creation success? {result}");

                    break;
                case ProfileContext.Mode.DELETE:

                    bool delResult = ProfileConfig.TryDelete(context.GetProfile());

                    sb.AppendLine($"{context.GetProfile()} profile deletion success? {delResult}");

                    break;
            }

            return sb.ToString();
        }

        public override string Run()
        {
            if (args.Length == 0)
            {
                return string.Empty;
            }

            switch (args[0])
            {
                case "-n":
                case "-name":

                    if (args.Length == 1)
                    {
                        return $"No value for {args[0]} parameter provided";
                    }

                    context.SetProfile(args[1]);

                    return new ProfileCommand([.. args.Skip(2)], context).Run();

                case "-m":
                case "-mode":

                    if (args.Length == 1)
                    {
                        return $"No value for {args[0]} parameter provided";
                    }

                    if (!Enum.TryParse(args[1], true, out ProfileContext.Mode result))
                    {
                        return $"{args[0]} is not a valid profile mode";
                    }

                    context.SetMode(result);

                    return new ProfileCommand([.. args.Skip(2)], context).Run();

                default:

                    return $"{args[0]} is an invalid argument";
            }
        }

        public override string GetCommand()
        {
            return "profile";
        }
    }
}
