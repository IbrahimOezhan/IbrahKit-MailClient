using MailClient.Configs;

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

        private void Execute()
        {
            switch (context.GetMode())
            {
                case ProfileContext.Mode.LIST:

                    ProfileConfig.GetAllConfigs().Where(x => x.Contains(context.GetName()));

                    break;
                case ProfileContext.Mode.CREATE:

                    ProfileConfig.TryCreate(context.GetName(), out _);

                    break;
                case ProfileContext.Mode.DELETE:

                    ProfileConfig.TryDelete(context.GetName());

                    break;
            }
        }

        public override string Run()
        {
            if (args.Length == 0)
            {
                Execute();
            }

            if (args.Length == 1)
            {
                return $"No value provided for {args[0]}";
            }

            switch (args[0])
            {
                case "-n":
                case "-name":

                    context.SetName(args[1]);

                    return new ProfileCommand(args.Skip(2).ToArray(), context).Run();

                case "-m":
                case "-mode":

                    if (!Enum.TryParse(args[1], out ProfileContext.Mode result))
                    {
                        return $"{args[0]} is not a valid profile mode";
                    }

                    context.SetMode(result);

                    return new ProfileCommand(args.Skip(2).ToArray(), context).Run();

                default:

                    return $"{args[0]} is an invalid argument";
            }
        }

        public override string CommandName()
        {
            return "profile";
        }
    }
}
