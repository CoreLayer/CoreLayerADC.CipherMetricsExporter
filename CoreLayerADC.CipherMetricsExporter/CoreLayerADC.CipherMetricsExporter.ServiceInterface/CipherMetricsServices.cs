using System.Collections.Generic;
using ServiceStack;
using CoreLayerADC.CipherMetricsExporter.ServiceModel;
using Prometheus;
using ServiceStack.Logging;

namespace CoreLayerADC.CipherMetricsExporter.ServiceInterface
{
    public class CipherMetricsServices : Service
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CipherMetricsServices));
        private static readonly Dictionary<string, Counter> Counters = new Dictionary<string, Counter>();

        public object Post(CipherHit request)
        {
            // Check if Metric exists
            if (!Counters.ContainsKey(request.ToJson()))
            {
                Counters.Add(
                    request.ToJson(),
                    Metrics.CreateCounter(
                        "cipher_hits_total", 
                        "Number of hits for a combination of TLS Version and Cipher",
                        new CounterConfiguration
                        {
                            LabelNames = new[]
                            {
                                "customer",
                                "node",
                                "vip",
                                "version",
                                "cipher"
                            }
                        }));
            }
            // Increase the counter
            Counters[request.ToJson()].WithLabels(new[]
                {request.Customer, request.Node, request.Vip, request.Version, request.Cipher}).Inc();
            
            Log.Debug(request.ToJson());
            return new CipherHitResponse();
        }
    }
}
