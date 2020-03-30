using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using OfflineMessaging.Api.IntegrationTests.Base;
using OfflineMessaging.Api.IntegrationTests.TestData;
using OfflineMessaging.Domain.Dtos.User;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OfflineMessaging.Api.IntegrationTests.User
{
    public class UserControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public UserControllerShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccessTrue_WhenParametersValid()
        {
            //Arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserDto>().Create();

            //Act
            var response = await _fixture.Client.PostAsync($"user/register", new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var sut = JsonConvert.DeserializeObject<UserRegisterResponseDto>(responseString);

            //Assert
            sut.Success.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(UserTestData.GetNonValidTestDataForRegister), MemberType = typeof(UserTestData))]
        public async Task RegisterAsync_ShouldReturnBadRequest_WhenParametersNotValid(string userName, string email, string password)
        {
            //Arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserDto>()
                .With(x => x.UserName, userName)
                .With(x => x.FirstName, "Test")
                .With(x => x.LastName, "User")
                .With(x => x.Email, email)
                .With(x => x.Password, password)
                .Create();

            //Act
            var response = await _fixture.Client.PostAsync($"user/register", new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json"));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccessTrue_WhenParametersValid()
        {
            //Arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserLoginParametersDto>()
                .With(x => x.UserName, "testuser")
                .With(x => x.Password, "pa$$w0rd!")
                .Create();

            //Act
            var response = await _fixture.Client.PostAsync($"user/login", new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var sut = JsonConvert.DeserializeObject<UserLoginResponseDto>(responseString);

            //Assert
            sut.Success.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(UserTestData.GetNonValidTestDataForLogin), MemberType = typeof(UserTestData))]
        public async Task LoginAsync_ShouldReturnBadRequest_WhenParametersNotValid(string userName, string password)
        {
            //Arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserLoginParametersDto>()
                .With(x => x.UserName, userName)
                .With(x => x.Password, password)
                .Create();

            //Act
            var response = await _fixture.Client.PostAsync($"user/login", new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json"));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
