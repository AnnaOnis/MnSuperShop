using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;


namespace MySyperShop.Domain.Tests
{
    public class AccountTests
    {
        private string _name = new Bogus.DataSets.Name().FullName();
        private string _email = new Bogus.DataSets.Internet().Email();
        private string _password = new Bogus.DataSets.Internet().Password();

        [Theory]
        [InlineData(null, "dfg@dfg.ru", "password")]
        [InlineData("name", null, "password")]
        [InlineData("name", "dfg@dfg.ru", null)]

        public async Task Account_with_null_parameter_will_not_be_registered(string name, string email, string password)
        {
            //Arrange
            var uowMock = new Mock<IUnitOfWork>();
            var passwordHasherMock = new Mock<IAppPasswordHasher>();
            var loger = new Mock<ILogger<AccountService>>();
            var accountService = new AccountService(uowMock.Object, passwordHasherMock.Object, loger.Object);

            //Act
            //Assert
            await FluentActions.Invoking(async () => await accountService.Register(name, email, password, default))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Account_is_registered()
        {
            //Arrange
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.AccountRepository.FindAccountByEmail(_email, CancellationToken.None))
                .ReturnsAsync((Account?)null);
            uowMock.Setup(u => u.CartRepository.Add(new Cart(Guid.NewGuid(), Guid.NewGuid()), default));

            var passwordHasherMock = new Mock<IAppPasswordHasher>();
            passwordHasherMock.Setup(h => h.HashPassword(_password)).Returns(_password).Verifiable();

            var logerMock = new Mock<ILogger<AccountService>>();

            var accountService = new AccountService(uowMock.Object, passwordHasherMock.Object, logerMock.Object);

            // 1-вариант
            //Act
            var account = await accountService.Register(_name, _email, _password, default);

            //Assert
            account.Email.Should().Be(_email);


            ////2-вариант
            ////Act
            //await accountService.Register(name, email, password, default);

            ////Assert
            //uowMock.Verify(it => it.SaveChangesAsync(default));
        }

        [Fact]
        public async Task Account_with_not_unique_email_will_not_be_registered()
        {
            // Arrange
            var existingAccount = new Account(Guid.NewGuid(), _name, _email, _password); // Существующий аккаунт

            var passwordHasherMock = new Mock<IAppPasswordHasher>();
            var logerMock = new Mock<ILogger<AccountService>>();
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.AccountRepository.FindAccountByEmail(_email, CancellationToken.None))
                .ReturnsAsync(existingAccount);

            var accountService = new AccountService(uowMock.Object, passwordHasherMock.Object, logerMock.Object);

            // Act & Assert
            await FluentActions.Invoking(async () => await accountService.Register(_name, _email, _password, CancellationToken.None))
                .Should().ThrowAsync<EmailAlreadyExistsException>();
        }
    }
}

