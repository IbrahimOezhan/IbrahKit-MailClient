using System.Text.Json;

namespace MailClient
{
    internal class HistoryHandler
    {
        private const string FOLDER = "IbrahKit";

        private const string FILE = "History.json";

        private string folder = string.Empty;

        private string file = string.Empty;

        private History history;

        public HistoryHandler()
        {
            folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FOLDER);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            file = Path.Combine(folder, FILE);

            if (!File.Exists(file))
            {
                File.Create(file).Close();
            }

            string fileContent = File.ReadAllText(file);

            if(fileContent == null || fileContent.Trim() == string.Empty)
            {
                history = new();
                return;
            }

            History? newHistory = JsonSerializer.Deserialize<History>(fileContent);

            if(newHistory == null)
            {
                throw new NullReferenceException();
            }

            history = newHistory;
        }

        public void AddToHistory(string adress)
        {
            history.AddToHistory(adress);
        }

        public void SaveHistory()
        {
            using StreamWriter sw = new(file);

            sw.Write(JsonSerializer.Serialize(history));
        }

        public bool Validate(List<string> adresses)
        {
            bool result = history.Validate(adresses);

            if (!result)
            {
                Console.WriteLine("Found duplicate adresses. Continue? Y/y (Yes) Anything Else (No)");

                ConsoleKeyInfo key = Console.ReadKey();

                Console.WriteLine();

                if (!key.KeyChar.ToString().Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
