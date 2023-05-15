using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests
{
    public class UserServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private Mock<IEmailService> _mockEmail;
        private Mock<IImageService> _mockImg;
        private Mock<IAccountService> _acco;
        [SetUp]
        public void Setup()
        {
            _mockRepository = new();
            _mockEmail = new();
            _mockImg = new();
            _acco = new();
        }

        [Test]
        public void GetMemberLevel_Test()
        {
            //Arrange
            List<decimal> testList = new List<decimal>() { 0, 500, 1000, 2000, 3000, 5000, 7500, 10000, 30000 };
            var expected = new List<MemberLevelDto> {
                new MemberLevelDto() { Level = "�~�ŷ|��", price = 0 } ,
                new MemberLevelDto() { Level = "�ջȷ|��", price = 1000 } ,
                new MemberLevelDto() { Level = "�ջȷ|��", price = 1000 } ,
                new MemberLevelDto() { Level = "�����|��", price = 3000 } ,
                new MemberLevelDto() { Level = "�����|��", price = 3000 } ,
                new MemberLevelDto() { Level = "�p�۷|��", price = 5000 } ,
                new MemberLevelDto() { Level = "�L��|��", price = 10000 } ,
                new MemberLevelDto() { Level = "�L��|��", price = 10000 } ,
                new MemberLevelDto() { Level = "", price = decimal.MaxValue } ,
            };
            var userService = new UserService(_mockRepository.Object, _mockEmail.Object, _mockImg.Object, _acco.Object);
            for (int i = 0; i < testList.Count; i++)
            {
                //Act
                var actual = userService.GetMemberLevel(testList[i]);

                //Assert
                Assert.That(actual, Is.EqualTo(expected[i]).Using(new MemberLevelDtoEqualityCompare()));
            }
        }
    }
    public class MemberLevelDtoEqualityCompare : IEqualityComparer<MemberLevelDto>
    {
        public bool Equals(Product? x, Product? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id && x.ProductName == y.ProductName;
        }

        public bool Equals(MemberLevelDto? x, MemberLevelDto? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Level == y.Level && x.price == y.price;
        }

        public int GetHashCode([DisallowNull] MemberLevelDto obj)
        {
            throw new NotImplementedException();
        }
    }
}