using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankApp
{
    public abstract class User : IEquatable<User>
    {
        private static int lastId = 0;

        #region Fields

        public int Id = lastId++;


        private string _firstName;

        protected string FirstName
        {
            get { return _firstName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Username cannot be null or empty");

                _firstName = value;
            }
        }

        private string _lastName;

        protected string LastName
        {
            get { return _lastName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("Username cannot be null or empty");

                _lastName = value;
            }

        }

        private UserSettings _userSettings;
        public UserSettings userSettings
        {
            get
            {
                return _userSettings;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("userSettings cannot be null");

                _userSettings = value;
            }
        }

        public List<Tuple<DateTime, string>> userLogs { get; } = new();

        #endregion //Fields

        public User(string firstName, string lastName, UserSettings settings)
        {
            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName) || settings == null)
                throw new ArgumentException("Argument cannot be null or empty, User() constructor");

            this.FirstName = firstName;
            this.LastName = lastName;
            this.userSettings = settings;
        }

        #region IEquatable

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            User objAsUser = obj as User;

            if (objAsUser == null)
                return false;

            return Equals(objAsUser);
        }

        public bool Equals(User otherUser)
        {
            if (otherUser == null)
                return false;
            return this.Id.Equals(otherUser.Id);
        }

        public override int GetHashCode()
        {
            return (int)Id % int.MaxValue;
        }

        #endregion //IEquatable

        public override string ToString()
        {
            StringBuilder strBuilder = new();

            strBuilder.AppendLine(this.FirstName + " " + this.LastName);
            strBuilder.AppendLine("User's settings: currency: " + this.userSettings.Currency + " language: " + this.userSettings.Language);

            foreach (Tuple<DateTime, string> log in this.userLogs)
            {
                strBuilder.AppendLine(log.Item1 + " " + log.Item2);
            }

            return strBuilder.ToString();
        }

        protected void WriteLog(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message cannot be null or empty, User.WriteLog()");

            this.userLogs.Add(new Tuple<DateTime, string>(DateTime.Now, message));
        }


    }
}
