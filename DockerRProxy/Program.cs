using DockerRProxy;

using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Model;

var builder = WebApplication.CreateBuilder(args);

List<RouteConfig> x = GetRoutes().ToList();
List<ClusterConfig> y = GetClusters("1").ToList();
var z = builder.Services.AddReverseProxy();
var a = z.LoadFromMemory(x, y);

x.AddRange(GetRoutes().ToList());
y.AddRange(GetClusters("2").ToList());

var k = builder.Services.BuildServiceProvider().GetService<IProxyConfigProvider>();
update.x = (InMemoryConfigProvider)k;
update.runUpdate(x,y);

var app = builder.Build();

app.MapReverseProxy();
app.Run();

RouteConfig[] GetRoutes()
{
    return new[]
    {
                new RouteConfig()
                {
                    RouteId = "route1",
                    ClusterId = "cluster1",
                    Match = new RouteMatch
                    {
                        // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
                        Path = "{**catch-all}"
                    }
                }
            };
}

ClusterConfig[] GetClusters(string x)
{
    var debugMetadata = new Dictionary<string, string>();
    debugMetadata.Add("DEBUG_METADATA_KEY", "DEBUG_VALUE");

    return new[]
    {
        new ClusterConfig()
        {
            ClusterId = x,
            SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "destination1", new DestinationConfig() { Address = "https://example.com" } },
                { "debugdestination1", new DestinationConfig() {
                    Address = "https://bing.com",
                    Metadata = debugMetadata  }
                },
            }
        }
    };
}