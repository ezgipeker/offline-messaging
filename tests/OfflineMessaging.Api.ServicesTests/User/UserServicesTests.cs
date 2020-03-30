using AutoFixture;
using FluentAssertions;
using Moq;
using OfflineMessaging.Api.Services.Token;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Api.ServicesTests.Base.Helpers;
using OfflineMessaging.Domain.Dtos.User;
using OfflineMessaging.Infrastructure.Context;
using System.Threading.Tasks;
using Xunit;

namespace OfflineMessaging.Api.ServicesTests.User
{
    public class UserServicesTests
    {
        private readonly IUserServices _userServices;
        private readonly Mock<ICheckUserServices> _checkUserServices;
        private readonly Mock<ICrudUserServices> _crudUserServices;
        private readonly Mock<ITokenServices> _tokenServices;
        public UserServicesTests()
        {
            var context = TestHelper.GetInMemoryDbContext<OfflineMessagingContext>();
            _checkUserServices = new Mock<ICheckUserServices>();
            _crudUserServices = new Mock<ICrudUserServices>();
            _tokenServices = new Mock<ITokenServices>();
            _userServices = new UserServices(context, _checkUserServices.Object, _crudUserServices.Object, _tokenServices.Object);
        }

        [Theory]
        [InlineData("testusername", "TestFirstName", "TestLastName", "testemail@test.com", "niBbRshzNi1q2F4FOwOq6wfWjvLHJSyJ974Hb8HMY34=ælBZAqjrEVpEmyvVsvzyOtA==")]
        public async Task RegisterAsync_ShouldReturnSuccessTrue_WhenParametersValid(string userName, string firstName, string lastName, string email, string password)
        {
            //Arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserDto>()
                .With(x => x.UserName, userName)
                .With(x => x.FirstName, firstName)
                .With(x => x.LastName, lastName)
                .With(x => x.Email, email)
                .With(x => x.Password, password)
                .Create();

            _checkUserServices.Setup(x => x.CheckUserExistByEmailAsync(email)).ReturnsAsync(false);
            _checkUserServices.Setup(x => x.CheckUserExistByUserNameAsync(userName)).ReturnsAsync(false);
            _crudUserServices.Setup(x => x.AddUserAsync(@object)).ReturnsAsync(true);

            //Act
            var sut = await _userServices.RegisterAsync(@object);

            //Assert
            sut.Success.Should().BeTrue();
            _checkUserServices.Verify(x => x.CheckUserExistByEmailAsync(It.Is<string>(b => b == email)), Times.Once);
            _checkUserServices.Verify(x => x.CheckUserExistByUserNameAsync(It.Is<string>(b => b == userName)), Times.Once);
            _crudUserServices.Verify(x => x.AddUserAsync(It.Is<UserDto>(b => b == @object)), Times.Once);
        }
    }
}
