using ServiceStack;

namespace CoreLayerADC.CipherMetricsExporter.ServiceModel
{
    [Route("/hit", "POST")]
    public class CipherHit : IReturn<CipherHitResponse>
    {
        public string Customer { get; set; }
        public string Node { get; set; }
        public string Vip { get; set; }
        public string Version { get; set; }
        public string Cipher { get; set; }
    }

    public class CipherHitResponse
    {
    }
}
