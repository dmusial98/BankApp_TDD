using System.Runtime.CompilerServices;

namespace BankApp
{
    public class UserSettings
    {
        public UserSettings() { }

        public UserSettings(string currency, string language)
        {
            this.Currency = currency;
            this.Language = language;
        }

        public string Currency { get; set; }

        public string Language { get; set; }


    }
}