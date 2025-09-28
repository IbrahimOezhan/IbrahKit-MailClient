using System.Text.Json.Serialization;

namespace MailClient
{
    public class MailClientRecepientConfig
    {
        [JsonInclude]
        private string adress;

        [JsonInclude]
        private List<string> formattings = new();


        public MailClientRecepientConfig()
        {

        }

        public MailClientRecepientConfig(string adress, List<string> formattings)
        {
            this.adress = adress;
            this.formattings = formattings;
        }

        public string GetAdress() => adress;

        public List<string> GetFormattings() => formattings;
    }
}