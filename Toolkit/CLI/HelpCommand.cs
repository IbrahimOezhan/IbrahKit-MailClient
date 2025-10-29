using System.Text;

namespace MailClient.Toolkit.CLI
{
    internal class HelpCommand : Command<HelpContext, HelpCommand>
    {
        public HelpCommand(string[] args) : base(args)
        {
        }

        public HelpCommand(string[] args, HelpContext context) : base(args, context)
        {
        }

        public override string Execute()
        {
            string commandName = GetContext().GetCommandName();

            bool res = commandName != string.Empty;
            

            IEnumerable<Type> commandTypes = CommandBase.GetAllCommands();
            
            List<CommandBase> commands = new();

            object[] args = [new string[] { "" }];

            foreach (var item in commandTypes)
            {
                CommandBase command = (CommandBase)Activator.CreateInstance(item, args);
            }

            if(res)
            {
                StringBuilder sb = new();

                CommandBase basealue = commands.Find(x => x.GetData().name == commandName);
                
                if(basealue != null)
                {
                    sb.Append(basealue.ToString());
                }

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new("The following commands are available\n\n");


                foreach (var item in commands)
                {
                    sb.Append(item.ToString());
                }

                return sb.ToString();
            }
        }

        public override (string,string,List<Argument>) GetData()
        {
            return ("help","returns a list of commands",[

                new Argument((args)=>
                {
                    GetContext().SetCommandName(args[1]);
                    return string.Empty;
                },"If you want help for one specific command","-c","-command")

            ]);
        }
    }
}
