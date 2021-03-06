using System;
using System.Security.Cryptography.X509Certificates;
using Couchbase.Core.IO.Authentication.X509;
using Xunit;

namespace Couchbase.UnitTests
{
    public class ClusterOptionsTests
    {
        #region ConfigPollInterval

        [Fact]
        public void Test_ConfigPollInterval_Default_Is_2_5Seconds()
        {
            var options =  new ClusterOptions();
            Assert.Equal(TimeSpan.FromSeconds(2.5), options.ConfigPollInterval);
        }

        [Fact]
        public void Test_EnableConfigPolling_Default_Is_True()
        {
            var options = new ClusterOptions();
            Assert.True(options.EnableConfigPolling);
        }

        #endregion

        #region EffectiveEnableTls

        [Theory]
        [InlineData(null, null, false)]
        [InlineData(null, "couchbase", false)]
        [InlineData(null, "http", false)]
        [InlineData(null, "couchbases", true)]
        [InlineData(false, null, false)]
        [InlineData(false, "couchbase", false)]
        [InlineData(false, "http", false)]
        [InlineData(false, "couchbases", false)]
        [InlineData(true, null, true)]
        [InlineData(true, "couchbase", true)]
        [InlineData(true, "http", true)]
        [InlineData(true, "couchbases", true)]
        public void EffectiveEnableTls_VariousSources_ExpectedResult(bool? enableTls, string scheme,
            bool expectedResult)
        {
            // Arrange

            var clusterOptions = new ClusterOptions
            {
                EnableTls = enableTls
            };

            if (scheme != null)
            {
                clusterOptions.WithConnectionString($"{scheme}://localhost");
            }

            // Act

            var result = clusterOptions.EffectiveEnableTls;

            // Assert

            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region x509

        [Fact]
        public void X509Certificate_Is_Null_By_Default()
        {
            var options = new ClusterOptions();

            Assert.Null(options.X509CertificateFactory);
        }

        [Fact]
        public void When_Set_X509Certificate_Is_NotNull_And_EnableTls_True()
        {
            var options = new ClusterOptions().
                WithX509CertificateFactory(CertificateFactory.GetCertificatesFromStore(
                new CertificateStoreSearchCriteria
                {
                    FindValue = "value",
                    X509FindType = X509FindType.FindBySubjectName,
                    StoreLocation = StoreLocation.CurrentUser,
                    StoreName = StoreName.CertificateAuthority
                }));

            Assert.NotNull(options.X509CertificateFactory);
            Assert.True(options.EffectiveEnableTls);
        }

        [Fact]
        public void When_X509Certificate_Is_Set_To_Null_Throw_NRE()
        {
           Assert.Throws<NullReferenceException>(()=> new ClusterOptions().WithX509CertificateFactory(null));
        }

        #endregion
    }
}
