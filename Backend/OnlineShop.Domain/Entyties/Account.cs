using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entyties
{
    public class Account : IEntity
    {
        private Guid _id;
        private string _name;
        private string _email;
        private string _password;

        public Account(Guid id, string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(message: "Value can not be null or whitespace. ", nameof(name)); ;
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(message: "Value can not be null or whitespace. ", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(message: "Value can not be null or whitespace. ", nameof(password));

            if (!new EmailAddressAttribute().IsValid(email))
                throw new ArgumentException(message: "Value is not a valid email address", nameof(email));

            _id = id;
            _name = name;
            _email = email;
            _password = password;
        }

        public Guid Id { get => _id; init => _id = value; }
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value can not be null or whitespace. ", nameof(value));
                _name = value;
            }
        }
        public string? Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value can not be null or whitespace. ", nameof(value));
                if (!new EmailAddressAttribute().IsValid(value))
                    throw new ArgumentException(message: "Value is not a valid email address", nameof(value));
                _email = value;
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value can not be null or whitespace. ", nameof(value));
                _password = value;
            }
        }
    }
}
