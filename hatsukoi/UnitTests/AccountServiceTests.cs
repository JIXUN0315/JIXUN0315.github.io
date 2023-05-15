using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    public class AccountServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private Mock<IHttpContextAccessor> _mockAcc;
        [SetUp]
        public void Setup()
        {
            _mockRepository = new();
            _mockAcc = new();
        }

        [Test]
        public void IsTimeMoreThan10min_Test()
        {
            //Arrange
            var sendTime = DateTime.UtcNow.AddMinutes(-5);
            var expected = true;
            //Act
            var accountService = new AccountService(_mockRepository.Object, _mockAcc.Object);
            var actual = accountService.IsTimeMoreThan10min(sendTime);

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
