using Funq;
using ServiceStack;
using NUnit.Framework;
using CoreLayerADC.CipherMetricsExporter.ServiceInterface;
using CoreLayerADC.CipherMetricsExporter.ServiceModel;

namespace CoreLayerADC.CipherMetricsExporter.Tests
{
    public class IntegrationTest
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost _appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(IntegrationTest), typeof(CipherMetricsServices).Assembly) { }

            public override void Configure(Container container)
            {
            }
        }

        public IntegrationTest()
        {
            _appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => _appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

        [Test]
        public void Can_call_Hello_Service()
        {
            var client = CreateClient();

            var response = client.Post(new CipherHit
            {
                Customer = "customer",
                Node = "node",
                Vip = "vip",
                Version = "version",
                Cipher = "cipher"
            });

            Assert.That(response.ToString(), Is.EqualTo("CoreLayerADC.CipherMetricsExporter.ServiceModel.CipherHitResponse"));
        }
    }
}