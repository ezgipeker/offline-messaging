using FluentAssertions;
using OfflineMessaging.Infrastructure.Extensions.Cryptography;
using OfflineMessaging.Infrastructure.Helpers;
using Xunit;

namespace OfflineMessaging.Infrastructure.Tests.Extensions.Cryptography
{
    public class CryptographyExtensionsTests
    {
        [Theory]
        [InlineData("pa$$w0rd!")]
        [InlineData("px~R!$6C")]
        [InlineData("R:sbw/u$d%wh4n67")]
        public void Pbkdf2Hash_ShouldHashedSame_ForSameSalt(string password)
        {
            var salt = CryptographyHelpers.CreateSalt();
            var hashedPassword1 = password.Pbkdf2Hash(salt);
            var hashedPassword2 = password.Pbkdf2Hash(salt);

            hashedPassword1.Should().Be(hashedPassword2);
        }

        [Theory]
        [InlineData("pa$$w0rd!", "CKBgEDtlvGR49dBEUN3uwHNmK42PKlxEgdJmwRD7IMQ=ærM1tqCXhDv+BgMz0AdOuOA==")]
        [InlineData("px~R!$6C", "vEGeyhr6wIMinnnxWyisXiCLSaPAHynnuCD+fCCY2tU=æbYqjmEs6QWHl9xh1YpzOiA==")]
        [InlineData("R:sbw/u$d%wh4n67", "niBbRshzNi1q2F4FOwOq6wfWjvLHJSyJ974Hb8HMY34=ælBZAqjrEVpEmyvVsvzyOtA==")]
        public void ValidateHash_ShouldReturnTrue_WhenPasswordAndHashCompatible(string password, string hash)
        {
            var result = password.ValidateHash(hash);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("pa$$w0rd!", "abc")]
        [InlineData("px~R!$6C", "xyzæMTIz")]
        [InlineData("R:sbw/u$d%wh4n67", "123æYWJj")]
        public void ValidateHash_ShouldReturnFalse_WhenPasswordAndHashNotCompatible(string password, string hash)
        {
            var result = password.ValidateHash(hash);

            result.Should().BeFalse();
        }
    }
}
