using System;
using System.Data;
using BankApp;
using IbanNet;
using IbanNet.Registry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/**
 * Klient: zasilenie konta w bankomacie
 * Klient: przelewy wychodzące / przychodzące (walidacja numeru na który wykonywany jest przelew (należy wymyślić własny format z "bitem parzystości")
 * Klient: kredyty (zasila konto o wybraną kwotę, konieczność spłaty wnioskowanej kwoty + %) 
 * Klient: przeglądanie historia operacji
 */

namespace BankAppUnitTests
{
    [TestClass]
    public class ClientTests
    {
        #region ATM

        [TestMethod]
        [DataRow(0.01)]
        [DataRow(10)]
        [DataRow(149.99)]
        [DataRow(1032.87)]
        public void BankAppClient_FundAccountATMWithZeroBalance_AccountFundCorrectSum(double amount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            decimal amountDec = (decimal)amount;
            decimal clientBalanceBeforeFund = client.Balance;
            client.FundAccountATM(amountDec);
            Assert.AreEqual(amountDec + clientBalanceBeforeFund, client.Balance);
        }

        [TestMethod]
        [DataRow(0.01)]
        [DataRow(10)]
        [DataRow(112.99)]
        [DataRow(501_032.87)]
        public void BankAppClient_FundAccountATMWithThirtyBalance_AccountFundCorrectSum(double amount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 30.0m);
            admin.AddClient(client);

            decimal amountDec = (decimal)amount;
            decimal clientBalanceBeforeFund = client.Balance;
            client.FundAccountATM(amountDec);
            Assert.AreEqual(amountDec + clientBalanceBeforeFund, client.Balance);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-0.01)]
        [DataRow(-149.99)]
        [DataRow(-1032.87)]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppClient_FundAccountATMWithZeroBalance_ThrowExceptionAccountFundWithIncorrectSum(double amount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            decimal amountDec = (decimal)amount;
            decimal clientBalanceBeforeFund = client.Balance;
            client.FundAccountATM(amountDec);
        }

        [TestMethod]
        [DataRow(0.01)]
        [DataRow(10)]
        [DataRow(149.99)]
        [DataRow(1000)]
        public void BankAppClient_DebitAccountATMWithOneThousandBalance_AccountDebitCorrectSum(double amount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal amountDec = (decimal)amount;
            decimal clientBalanceBeforeFund = client.Balance;
            client.DebitAccountATM(amountDec);
            Assert.AreEqual(clientBalanceBeforeFund - amountDec, client.Balance);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-0.01)]
        [DataRow(-149.99)]
        [DataRow(1032.87)]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppClient_DebitAccountATMWithOneThousandBalance_ThrowExceptionAccountFundWithIncorrectSum(double amount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            decimal amountDec = (decimal)amount;
            decimal clientBalanceBeforeFund = client.Balance;
            client.DebitAccountATM(amountDec);
        }
        #endregion

        #region Transfer

        [TestMethod]
        [DataRow(0.01)]
        [DataRow(0.99)]
        [DataRow(100.0)]
        [DataRow(999.99)]
        [DataRow(1000.0)]
        public void BankAppClient_MakeTransfer_MakeCorrectTransfer(double money)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            decimal balanceUserFromBeforeTransfer = clientFrom.Balance;
            decimal balanceUserToBeforeTransfer = clientTo.Balance;
            decimal moneyDec = (decimal)money;

            clientFrom.MakeTransfer(clientTo.IBAN, moneyDec, clientTo);

            Assert.AreEqual(clientFrom.Balance, balanceUserFromBeforeTransfer - moneyDec);
            Assert.AreEqual(clientTo.Balance, balanceUserToBeforeTransfer + moneyDec);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppClient_MakeTransfer_ThrowExceptionIbanNullPassed()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            clientFrom.MakeTransfer(null, 10.0m, clientTo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppClient_MakeTransfer_ThrowExceptionNullUserToPassed()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            clientFrom.MakeTransfer(clientTo.IBAN, 10.0m, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppClient_MakeTransfer_ThrowExceptionIncorrectIBANByUserTo()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            Iban iban;
            IIbanParser ibanParser = new IbanParser(IbanRegistry.Default);
            bool successIBAN = ibanParser.TryParse("PL 75 1234 5679 0000 1111 2222 3333", out iban); //make new correct IBAN
            clientFrom.MakeTransfer(iban, 10.0m, clientTo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppClient_MakeTransfer_ThrowExceptionWithNotEnoughBalance()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 150.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            clientFrom.MakeTransfer(clientTo.IBAN, 200.0m, clientTo);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-0.01)]
        [DataRow(-100_000.0)]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppClient_MakeTransfer_ThrowExceptionMoneyLessThanZero(double money)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 150.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 0.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            decimal moneyDecimal = (decimal)money;

            clientFrom.MakeTransfer(clientTo.IBAN, moneyDecimal, clientTo);
        }

        #endregion

        #region Loan

        [TestMethod]
        [DataRow(1000, 5, 206, 1030)]
        [DataRow(250, 10, 25.75, 257.50)]
        [DataRow(10_000, 20, 525, 10_500)]
        [DataRow(30_000, 30, 1080, 32400)]
        [DataRow(80_000, 40, 2_240, 89_600)]
        [DataRow(500_000, 100, 5750, 575_000)]
        public void BankAppClient_TakeLoan_GetCorrectLoan(double loanMoney, int monthsAmount, double monthlyRepayment, double debt)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal loanMoneyDecimal = (decimal) loanMoney;
            decimal monthlyRepaymentDecimal = (decimal) monthlyRepayment;
            decimal debtDecimal = (decimal) debt;
            client.TakeLoan(loanMoneyDecimal, monthsAmount);

            Assert.AreEqual(monthlyRepaymentDecimal, client.MonthlyRepayment);
            Assert.AreEqual(debtDecimal, client.Debt);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-0.01)]
        public void BankAppClient_TakeLoan_ThrowExceptionLoanLessThanZero(double money)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal moneyDecimal = (decimal) money;
            client.TakeLoan(moneyDecimal, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-100)]
        public void BankAppClient_TakeLoan_ThrowExceptionMonthsAmountLessThanZero(int monthsAmount)
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            client.TakeLoan(100.0m, monthsAmount);
        }

        #endregion

        #region ClientLogs

        [TestMethod]
        public void BankAppClient_UserLogs_CreateClient()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            string clientLogs = client.ToString();
            StringAssert.Contains(clientLogs, "Created bank account.");
        }

        [TestMethod]
        public void BankAppClient_UserLogs_FundAccountATM()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            decimal moneyToFundAccountViaATM = 100.0m;
            client.FundAccountATM(moneyToFundAccountViaATM);

            string clientLogs = client.ToString();
            StringAssert.Contains(clientLogs, $"User fund account via ATM with {moneyToFundAccountViaATM}{client.userSettings.Currency}.");
        }

        [TestMethod]
        public void BankAppClient_UserLogs_DebitAccountATM()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal moneyToDebitAccountViaATM = 100.0m;
            client.DebitAccountATM(moneyToDebitAccountViaATM);

            string clientLogs = client.ToString();
            StringAssert.Contains(clientLogs,
                $"User debit account via ATM with {moneyToDebitAccountViaATM}{client.userSettings.Currency}.");
        }

        [TestMethod]
        public void BankAppClient_UserLogs_MakeTransfer()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 150.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            decimal moneyToTransfer = 256.99m;
            clientFrom.MakeTransfer(clientTo.IBAN, moneyToTransfer, clientTo);

            string clientFromLogs = clientFrom.ToString();
            StringAssert.Contains(clientFromLogs, $"Client sent {moneyToTransfer}{clientFrom.userSettings.Currency} to {clientTo.FirstName} {clientTo.LastName} IBAN: {clientTo.IBAN}");
            string clientToLogs = clientTo.ToString();
            StringAssert.Contains(clientToLogs, $"User {clientFrom.FirstName} {clientFrom.LastName} {clientFrom.IBAN} sent {moneyToTransfer}{clientFrom.userSettings.Currency}");
        }

        [TestMethod]
        public void BankAppClient_UserLogs_TakeLoan()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal moneyInLoan = 1000.0m;
            client.TakeLoan(moneyInLoan, 12);
            string userLogs = client.ToString();
            StringAssert.Contains(userLogs, $"User took a loan with {moneyInLoan}{client.userSettings.Currency}");
        }

        #endregion
    }
}
