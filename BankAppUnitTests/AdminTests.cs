using System;
using System.Collections.Generic;
using BankApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//unitOfWorkName_ScenarioUnderTest_ExpectedBehaviour

namespace BankAppUnitTests
{
    [TestClass]
    public class AdminTests :TestBase
    {
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


        [TestCleanup]
        public void TestCleanup()
        {
            
        }
    }
}
