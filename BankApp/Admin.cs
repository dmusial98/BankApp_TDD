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
            this.bank = bank ?? throw new ArgumentNullException(nameof(bank));
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

        public void AddClient(Client newClient)
        {
            if (newClient == null)
                throw new ArgumentNullException();

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

            this.bank.Admins.Remove(adminToDelete);
        }

        public void DeleteAdmin(int id)
        {
            if (id < 0)
                throw new ArgumentException("id cannot be less than zero.");

            this.bank.Admins.Remove(this.bank.Admins.Find(x => x.Id == id));
        }

        public void DeleteClient(Client clientToDelete)
        {
            if (clientToDelete == null)
                throw new ArgumentNullException();

            this.bank.Clients.Remove(clientToDelete);
        }

        public void DeleteClient(int id)
        {
            if (id < 0)
                throw new ArgumentException("id cannot be less than zero.");

            this.bank.Clients.Remove(this.bank.Clients.Find(x => x.Id == id));
        }

        public void ChangeUserSettings(User user, UserSettings newUserSettings)
        {
            if (user == null || newUserSettings == null)
                throw new ArgumentNullException("Arguments cannot be null, Admin.changeUserSettings()");

            Admin admin = null;
            Client client = null;
            bool isAdmin = true, isClient = true;
            try
            {
                admin = (Admin) user;
            }
            catch (InvalidCastException ex)
            {
                isAdmin = false;
            }

            try
            {
                client = (Client)user;
            }
            catch (InvalidCastException ex)
            {
                isClient = false;
            }

            if (!isClient && !isAdmin)
                throw new ArgumentException("User isn't admin or client");

            if (isAdmin)
            {
                admin = this.bank.Admins.Find(x => x.Id == admin.Id);
                if (admin != null)
                    admin.userSettings = newUserSettings;
                else
                    throw new DataException("bank doesn't contain that admin");
            }

            if (isClient)
            {
                client = this.bank.Clients.Find(x => x.Id == client.Id);
                if(client != null)
                    client.userSettings = newUserSettings;
                else
                    throw new DataException("bank doesn't contain that client");
            }
        }

        public string GetUserLogs(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Argument cannot be null, Admin.GetUserLogs()");

            return user.ToString();
        }
    }
}
