using System.Runtime.Serialization;

namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class EmailAlreadyExistsException : DomainException
    {
        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }
    }
}