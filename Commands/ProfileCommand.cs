using MailClient.Configs;
using MailClient.Toolkit.CLI;
using MailClient.Toolkit.CLI.Exceptions;
using System.Text;

namespace MailClient.Commands
{
    internal class ProfileCommand : Command<ProfileContext, ProfileCommand>
    {
        public ProfileCommand(string[] args) : base(args) { }

        public ProfileCommand(string[] args, ProfileContext context) : base(args, context) { }

        public override string Execute()
        {
            StringBuilder sb = new();

            switch (GetContext().GetMode())
            {
                case ProfileContext.Mode.LIST:

                    sb.AppendLine("Found the following profiles:");

                    foreach (var profile in ProfileConfig.GetConfigs().Where(
                    x => x.GetProfileName().Contains(GetContext().GetProfile(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sb.AppendLine(profile.ToString());
                    }

                    break;
                case ProfileContext.Mode.CREATE:

                    bool result = ProfileConfig.TryCreate(GetContext().GetProfile(), out _);

                    sb.AppendLine($"{GetContext().GetProfile()} profile creation success? {result}");

                    break;
                case ProfileContext.Mode.DELETE:

                    bool delResult = ProfileConfig.TryDelete(GetContext().GetProfile());

                    sb.AppendLine($"{GetContext().GetProfile()} profile deletion success? {delResult}");

                    break;
            }

            return sb.ToString();
        }

        public override (string name,string desc,List<Argument> args) GetData()
        {
            return
            ("profile","execute various actions with profiles",[
                new((args) =>
                {
                    GetContext().SetProfile(args[1]);

                    return string.Empty;

                },"Set the name of the profile to use for this action","-p","-profile"),
                new((args)=>
                {
                    if (!Enum.TryParse(args[1], true, out ProfileContext.Mode result))
                    {
                        throw new ArgumentParsingException($"{args[0]} is not a valid profile mode");
                    }

                    GetContext().SetMode(result);

                    return string.Empty;

                },"Set the mode to use for the action","-m","-mode"),
            ]);
        }
    }
}
