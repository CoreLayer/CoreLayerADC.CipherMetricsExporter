using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using CoreLayerADC.CipherMetricsExporter.ServiceInterface;
using CoreLayerADC.CipherMetricsExporter.ServiceModel;

namespace CoreLayerADC.CipherMetricsExporter.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost _appHost;

        public UnitTest()
        {
            _appHost = new BasicAppHost().Init();
            _appHost.Container.AddTransient<CipherMetricsServices>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => _appHost.Dispose();

        [Test]
        public void Can_call_MyServices()
        {
            var service = _appHost.Container.Resolve<CipherMetricsServices>();

            var response = (CipherHitResponse)service.Post(new CipherHit {
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
