using System;
using System.Collections.Generic;
using System.Data;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//unitOfWorkName_ScenarioUnderTest_ExpectedBehaviour

namespace BankAppUnitTests
{
    [TestClass]
    public class AdminTests : TestBase
    {

        #region Manage admins list

        [TestMethod]
        public void BankAppAdmin_AddAdmin_AdminsAreTheSame()
        {
            Bank bank = new Bank();

            Admin admin1 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Admin admin2 = new Admin("second", "admin", new UserSettings("PLN", "Polish"), bank);

            Admin adminFromBank1 = admin1.GetAdmin(admin1.Id);
            Admin adminFromBank2 = admin1.GetAdmin(admin2.Id);

            Assert.AreSame(admin1, adminFromBank1);
            Assert.AreSame(admin2, adminFromBank2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppAdmin_AddAdmin_ThrowExceptionBankContainsThatAdmin()
        {
            Bank bank = new Bank();
            Admin admin1 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            admin1.AddAdmin(admin1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_AddAdmin_ThrowExceptionPassedNull()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Admin nullAdmin = null;
            admin.AddAdmin(nullAdmin);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_AddAdmin_ThrowExceptionNullReferenceBank()
        {
            Admin admin1 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), null);
        }

        [TestMethod]
        public void BankAppAdmin_DeleteAdminWithReference_BankDoesNotContainAdmin()
        {
            Bank bank = new Bank();

            Admin admin1 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Admin admin2 = new Admin("second", "admin", new UserSettings("PLN", "Polish"), bank);

            Admin adminFromBank1 = admin1.GetAdmin(admin1.Id);
            Admin adminFromBank2 = admin1.GetAdmin(admin2.Id);

            Assert.AreSame(admin1, adminFromBank1);
            Assert.AreSame(admin2, adminFromBank2);

            admin1.DeleteAdmin(admin2);

            adminFromBank1 = admin1.GetAdmin(admin2.Id);

            Assert.IsNull(adminFromBank1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_DeleteAdminWithReference_ThrowExceptionPassedNullAdmin()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Admin nullAdmin = null;
            admin.DeleteAdmin(nullAdmin);
        }

        [TestMethod]
        public void BankAppAdmin_DeleteAdminWithId_BankDoesNotContainAdmin()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Admin admin2 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);

            Admin adminFromBank1 = admin.GetAdmin(admin.Id);
            Admin adminFromBank2 = admin.GetAdmin(admin2.Id);

            Assert.AreSame(admin, adminFromBank1);
            Assert.AreSame(admin2, adminFromBank2);

            admin.DeleteAdmin(admin2.Id);

            adminFromBank1 = admin.GetAdmin(admin2.Id);

            Assert.IsNull(adminFromBank1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppAdmin_DeleteAdminWithId_ThrowExceptionPassedIdLessThanZero()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            admin.DeleteAdmin(-1);
        }

        #endregion

        #region Manage clients list
        [TestMethod]
        public void BankAppAdmin_AddClient_ClientsAreTheSame()
        {
            Bank bank = new Bank();

            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("second", "admin", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            Client clientFromBank = admin.GetClient(client.Id);

            Assert.AreSame(client, clientFromBank);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppAdmin_AddClient_ThrowExceptionBankContainsThatClient()
        {
            Bank bank = new Bank();
            Admin admin1 = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("second", "admin", new UserSettings("PLN", "Polish"));
            admin1.AddClient(client);
            admin1.AddClient(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_AddAClient_ThrowExceptionPassedNull()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client nullClient = null;
            admin.AddClient(nullClient);
        }

        [TestMethod]
        public void BankAppAdmin_DeleteClientWithReference_BankDoesNotContainClient()
        {
            Bank bank = new Bank();

            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("second", "admin", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            Client clientFromBank = admin.GetClient(client.Id);
            Assert.AreSame(client, clientFromBank);

            admin.DeleteClient(client);

            clientFromBank = admin.GetClient(client.Id);
            Assert.IsNull(clientFromBank);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_DeleteClientWithReference_ThrowExceptionPassedNullClient()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client nullClient = null;
            admin.DeleteClient(nullClient);
        }

        [TestMethod]
        public void BankAppAdmin_DeleteClientWithId_BankDoesNotContainClient()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            Client clientFromBank = admin.GetClient(client.Id);
            Assert.AreSame(client, clientFromBank);

            admin.DeleteClient(client.Id);

            clientFromBank = admin.GetClient(client.Id);
            Assert.IsNull(clientFromBank);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAppAdmin_DeleteClientWithId_ThrowExceptionPassedIdLessThanZero()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            admin.DeleteClient(-1);
        }

        #endregion

        #region Change user settings

        [TestMethod]
        public void BankAppAdmin_ChangeUserSettings_SettingsAreTheSame()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PL", "Polish"));
            admin.AddClient(client);
            UserSettings newUserSettings = new("GBP", "English");

            admin.ChangeUserSettings(client, newUserSettings);
            Assert.AreSame(client.userSettings, newUserSettings);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_ChangeUserSettings_ThrowExceptionPassedNullSettings()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PL", "Polish"));
            admin.AddClient(client);

            admin.ChangeUserSettings(client, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BankAppAdmin_ChangeUserSettings_ThrowExceptionPassedNullUser()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PL", "Polish"));
            admin.AddClient(client);

            admin.ChangeUserSettings(null, new UserSettings("GBP", "English"));
        }

        [TestMethod]
        [ExpectedException(typeof(DataException))]
        public void BankAppAdmin_ChangeUserSettings_ThrowExceptionBankDoesNotContainUser()
        {
            Bank bank = new Bank();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PL", "Polish"));
            Client client2 = new Client("second", "client", new UserSettings("PL", "Polish"));
            admin.AddClient(client);

            admin.ChangeUserSettings(client2, new UserSettings("GBP", "English"));
        }

        #endregion


        //TODO:UserLogs
        #region Userlogs

        [TestMethod]
        public void BankAppAdmin_UserLogs_CreateClient()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            string clientLogs = admin.GetUserLogs(client);
            StringAssert.Contains(clientLogs, "Created bank account.");
        }

        [TestMethod]
        public void BankAppAdmin_UserLogs_FundAccountATM()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"));
            admin.AddClient(client);

            decimal moneyToFundAccountViaATM = 100.0m;
            client.FundAccountATM(moneyToFundAccountViaATM);

            string clientLogs = admin.GetUserLogs(client);
            StringAssert.Contains(clientLogs, $"User fund account via ATM with {moneyToFundAccountViaATM}{client.userSettings.Currency}.");
        }

        [TestMethod]
        public void BankAppAdmin_UserLogs_DebitAccountATM()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal moneyToDebitAccountViaATM = 100.0m;
            client.DebitAccountATM(moneyToDebitAccountViaATM);

            string clientLogs = admin.GetUserLogs(client);
            StringAssert.Contains(clientLogs,
                $"User debit account via ATM with {moneyToDebitAccountViaATM}{client.userSettings.Currency}.");
        }

        [TestMethod]
        public void BankAppAdmin_UserLogs_MakeTransfer()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client clientFrom = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            Client clientTo = new Client("second", "client", new UserSettings("PLN", "Polish"), 150.0m);
            admin.AddClient(clientFrom);
            admin.AddClient(clientTo);

            decimal moneyToTransfer = 256.99m;
            clientFrom.MakeTransfer(clientTo.IBAN, moneyToTransfer, clientTo);

            string clientFromLogs = admin.GetUserLogs(clientFrom);
            StringAssert.Contains(clientFromLogs, $"Client sent {moneyToTransfer}{clientFrom.userSettings.Currency} to {clientTo.FirstName} {clientTo.LastName} IBAN: {clientTo.IBAN}");
            string clientToLogs = admin.GetUserLogs(clientTo);
            StringAssert.Contains(clientToLogs, $"User {clientFrom.FirstName} {clientFrom.LastName} {clientFrom.IBAN} sent {moneyToTransfer}{clientFrom.userSettings.Currency}");
        }

        [TestMethod]
        public void BankAppAdmin_UserLogs_TakeLoan()
        {
            Bank bank = new();
            Admin admin = new Admin("first", "admin", new UserSettings("PLN", "Polish"), bank);
            Client client = new Client("new", "client", new UserSettings("PLN", "Polish"), 1000.0m);
            admin.AddClient(client);

            decimal moneyInLoan = 1000.0m;
            client.TakeLoan(moneyInLoan, 12);
            string userLogs = admin.GetUserLogs(client);
            StringAssert.Contains(userLogs, $"User took a loan with {moneyInLoan}{client.userSettings.Currency}");
        }

        #endregion
    }
}
