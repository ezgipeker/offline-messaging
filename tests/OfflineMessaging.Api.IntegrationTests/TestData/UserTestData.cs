using System.Collections.Generic;

namespace OfflineMessaging.Api.IntegrationTests.TestData
{
    public static class UserTestData
    {
        public static IEnumerable<object[]> GetNonValidTestDataForRegister()
        {
            return new[]
            {
                new object[] { string.Empty, "ezgi.peker.6@gmail.com", "CKBgEDtlvGR49dBEUN3uwHNmK42PKlxEgdJmwRD7IMQ=ærM1tqCXhDv+BgMz0AdOuOA==" },
                new object[] { "ezgipeker", string.Empty, "CKBgEDtlvGR49dBEUN3uwHNmK42PKlxEgdJmwRD7IMQ=ærM1tqCXhDv+BgMz0AdOuOA==" },
                new object[] { "ezgipeker", "ezgi.peker.6@gmail.com", string.Empty },
                new object[] { string.Empty, string.Empty, string.Empty }
            };
        }

        public static IEnumerable<object[]> GetNonValidTestDataForLogin()
        {
            return new[]
            {
                new object[] { string.Empty, "CKBgEDtlvGR49dBEUN3uwHNmK42PKlxEgdJmwRD7IMQ=ærM1tqCXhDv+BgMz0AdOuOA==" },
                new object[] { "ezgipeker", string.Empty },
                new object[] { string.Empty, string.Empty }
            };
        }
    }
}
