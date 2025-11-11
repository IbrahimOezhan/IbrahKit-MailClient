using IbrahKit_CLI;
using IbrahKit_CLI.Params;
using IbrahKit_MailClient.Configs;
using IbrahKit_MailClient.Utilities;
using System.Text;

namespace IbrahKit_MailClient.Commands
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

        public override (string name, string desc, List<Param> args) GetData()
        {
            return
            ("profile", "Execute various actions with profiles",
            [
                new Argument((args) =>
                {
                    GetContext().SetProfile(args[1]);

                    return ARG_PROCESS_SUCCES;

                },"Set the name of the profile to use for this action",
                "-p","-profile"),

                new Argument((args)=>
                {
                    GetContext().SetMode(args[1]);

                    return ARG_PROCESS_SUCCES;

                },$"Set the mode to use for the action. The following are available: {CollectionUtilities.OutputEnum<ProfileContext.Mode>(typeof(ProfileContext.Mode))}",
                "-m","-mode"),
            ]);
        }
    }
}
