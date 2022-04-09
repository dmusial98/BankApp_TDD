using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Admin : User
    {
        public Bank bank;

        public Admin(string firstName, string lastName, UserSettings settings, Bank bank)
            : base(firstName, lastName, settings)
        {
            if(bank == null)
                throw new ArgumentNullException(nameof(bank));

            this.bank = bank;
            this.bank.Admins.Add(this);
        }

        public void AddAdmin(Admin newAdmin)
        {
            if (newAdmin == null)
                throw new ArgumentNullException("Argument cannot be null, Admin.AddAdmin()");

            if (this.bank.Admins.Contains(newAdmin))
                throw new ArgumentException("Admin list contains that admin.");

            this.bank.Admins.Add(newAdmin);
        }

        public void AddClient(List<Client> clients, Client newClient)
        {
            if (clients == null || newClient == null)
                throw new ArgumentNullException("Argument cannot be null, Admin.AddAdmin()");

            if (this.bank.Clients.Contains(newClient))
                throw new ArgumentException("Client list contains that client.");

            this.bank.Clients.Add(newClient);
        }

        public Admin GetAdmin(int id) => this.bank.Admins.Find(x => x.Id == id);
        
        public Client GetClient(int id) => this.bank.Clients.Find(x => x.Id == id);

        public void DeleteAdmin(Admin adminToDelete)
        {
            if (adminToDelete == null)
                throw new ArgumentNullException();

            //if (this.bank.Admins.Count == 0)
            //    throw new DataException("Cannot delete user when userList is empty, Admin.DeleteAdmin()");

            this.bank.Admins.Remove(adminToDelete);
        }

        public void DeleteAdmin(int id)
        {
            if (id < 0)
                throw new ArgumentException("id cannot be less than zero.");

            this.bank.Admins.Remove(this.bank.Admins.Find(x => x.Id == id));
        }

        public void DeleteClient(List<Client> clients, Client clientToDelete)
        {
            if (clients == null || clientToDelete == null)
                throw new ArgumentNullException("Argument cannot be null, Admin.DeleteClient");

            if (clients.Count == 0)
                throw new ArgumentException("Cannot delete user when userList is empty, Admin.DeleteClient()");

            clients.Remove(clientToDelete);
        }

        public void ChangeUserSettings(List<User> userList, User user, UserSettings newUserSettings)
        {
            if (user == null || newUserSettings == null || userList == null)
                throw new ArgumentNullException("Arguments cannot be null, Admin.changeUserSettings()");

            if (userList.Count == 0)
                throw new ArgumentException("Users list cannot be empty");

            var userToChange = userList.Find(x => x.Id == user.Id);

            if (userToChange != null)
                userToChange.userSettings = userSettings;
            else
                throw new ArgumentOutOfRangeException("cannot find user, Admin.ChangeUserSettings()");
        }

        public string GetUserLogs(List<User> userList, User user)
        {
            if (userList == null || user == null)
                throw new ArgumentNullException("Argument cannot be null, Admin.GetUserLogs()");

            var userForLogs = userList.Find(x => x.Id == user.Id);

            if (userForLogs == null)
                throw new ArgumentOutOfRangeException("cannot find user, Admin.GetUserLogs");
            
            return userForLogs.ToString();
        }
    }
}
