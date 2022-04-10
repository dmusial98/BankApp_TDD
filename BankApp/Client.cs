using IbanNet;
using IbanNet.Registry;
using System;
using System.Data;

namespace BankApp
{
    public class Client : User
    {
        public decimal Balance { get; set; }

        public Iban IBAN { get; }

        public decimal MonthlyRepayment { get; private set; }

        public decimal Debt { get; private set; }

        public Client(string firstName, string lastName, UserSettings settings, decimal balance = 0.0m)
            : base(firstName, lastName, settings)
        {
            this.Balance = balance;

            decimal ibanChecksum = CountIbanChecksum(this.Id);

            string IBANstr = /*"PL 75 1234 5679 0000 1111 2222 3333";*/
                ("PL " + ibanChecksum.ToString("##.###").PadLeft(2, '0') + " " + this.Id.ToString("##.###").PadLeft(24, '0'))
                .Insert(10, " ")
                .Insert(15, " ")
                .Insert(20, " ")
                .Insert(25, " ")
                .Insert(30, " ");

            Iban iban;
            if (ValidateIban(IBANstr, out iban))
                this.IBAN = iban;
            else
                throw new DataException("IBAN isn't correct");

            this.WriteLog("Created bank account.");
        }

        private decimal CountIbanChecksum(decimal id)
        {
            string numberStr = (id.ToString().PadLeft(24, '0'));
            decimal parsedNumberFromStr;
            decimal moduloResult;

            if (Decimal.TryParse(numberStr, out parsedNumberFromStr))
            {
                moduloResult = parsedNumberFromStr % 97;
                moduloResult *= 1_000_000.0M;
                moduloResult += 94;
                moduloResult %= 97;
            }
            else
                throw new DataException("cannot count checksum for IBAN");

            return 98.0M - moduloResult;
        }

        private bool ValidateIban(string ibanStr, out Iban iban)
        {
            IIbanParser ibanParser = new IbanParser(IbanRegistry.Default);
            bool successIBAN = ibanParser.TryParse(ibanStr, out iban);
            if (successIBAN)
                return true;

            return false;
        }

        public void FundAccountATM(decimal money)
        {
            if (money <= 0.0m)
                throw new ArgumentException("Amount cannot be less or equal zero");

            this.Balance += money;

            this.WriteLog($"User fund account via ATM with {money}{this.userSettings.Currency}.");
        }

        public void DebitAccountATM(decimal money)
        {
            if (money <= 0.0m)
                throw new ArgumentException("Amount cannot equals zero");

            if (this.Balance < money)
                throw new ArgumentException("cannot debit more many than client has in balance");

            this.Balance -= money;

            this.WriteLog($"User debit account via ATM with {money}{this.userSettings.Currency}.");
        }

        public void MakeTransfer(Iban ibanAccountTo, decimal money, Client userTo)
        {
            if (ibanAccountTo == null || userTo == null || ibanAccountTo == null)
                throw new ArgumentNullException();

            if (money <= 0)
                throw new ArgumentException("Cannot transfer money less or equal zero, Client.MakeTransfer()");

            if (this.Balance < money)
                throw new ArgumentException("Client balance don't have money, Client.MakeTransfer()");

            if (userTo.IBAN != ibanAccountTo)
                throw new ArgumentException("IBAN number and User to don't match, Client.MakeTransfer()");

            this.Balance -= money;
            userTo.Balance += money;

            this.WriteLog($"Client sent {money}{this.userSettings.Currency} to {userTo.FirstName} {userTo.LastName} IBAN: {userTo.IBAN}");
            userTo.WriteLog($"User {this.FirstName} {this.LastName} {this.IBAN} sent {money}{this.userSettings.Currency}");
        }

        public void TakeLoan(decimal money, int monthsAmount)
        {
            if (money <= 0.0M)
                throw new ArgumentException("Money amount cannot be less or equal zero.");
            if (monthsAmount <= 0)
                throw new ArgumentException("months amount cannot be less or equals zero.");

            decimal interestPercentage;

            if (monthsAmount <= 12)
                interestPercentage = 3.0M;
            else if (monthsAmount <= 24)
                interestPercentage = 5.0M;
            else if (monthsAmount <= 36)
                interestPercentage = 8.0M;
            else if (monthsAmount <= 48)
                interestPercentage = 12.0M;
            else
                interestPercentage = 15.0M;

            Debt += Math.Round((money * (100.0M + interestPercentage) / 100.0M), 2);

            MonthlyRepayment += Math.Round(((money + (money * interestPercentage / 100.0M)) / monthsAmount), 2);

            WriteLog($"User took a loan with {money}{this.userSettings.Currency}");

        }
    }
}
