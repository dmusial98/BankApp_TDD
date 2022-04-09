using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Bank
    {
        private List<Admin> _admins = new();

        private List<Client> _clients = new();

        public List<Admin> Admins
        {
            get { return _admins; }
        }

        public List<Client> Clients
        {
            get { return _clients; }
        }

        //public void GetClient(int id, out Client client, Admin adminWhoHasAccess)
        //{
        //    if (Admins.Contains(adminWhoHasAccess))
        //        client = _clients.Find(x => x.Id == id);
        //    else
        //        throw new ArgumentException("admin doesn't have access to bank");
        //}

        //public void GetAdmin(int id, out Admin admin, Admin adminWhoHasAccess)
        //{
        //    if (_admins.Contains(adminWhoHasAccess))
        //        admin = _admins.Find(x => x.Id == id);
        //    else
        //        throw new ArgumentException("admin doesn't have access to bank");
        //}
    }
}
