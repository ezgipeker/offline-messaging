using AutoFixture;
using FluentAssertions;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Api.ServicesTests.Base.Helpers;
using OfflineMessaging.Domain.Dtos.User;
using OfflineMessaging.Infrastructure.Context;
using System.Threading.Tasks;
using Xunit;

namespace OfflineMessaging.Api.ServicesTests.User
{
    public class CrudUserServicesTests
    {
        private readonly ICrudUserServices _crudUserServices;
        public CrudUserServicesTests()
        {
            var context = TestHelper.GetInMemoryDbContext<OfflineMessagingContext>();
            _crudUserServices = new CrudUserServices(context);
        }

        [Theory]
        [InlineData("username", "FirstName", "LastName", "email@test.com", "niBbRshzNi1q2F4FOwOq6wfWjvLHJSyJ974Hb8HMY34=ælBZAqjrEVpEmyvVsvzyOtA==")]
        public async Task AddUserAsync_ShouldReturnSuccessTrue_WhenParametersValid(string userName, string firstName, string lastName, string email, string password)
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

            //Act
            var sut = await _crudUserServices.AddUserAsync(@object);

            //Assert
            sut.Should().BeTrue();
        }
    }
}
