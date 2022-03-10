using Docker.DotNet.Models;
using Docker.DotNet;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Model;

namespace DockerRProxy;

static class update
{
    public static InMemoryConfigProvider x;

    public static void runUpdate(List<RouteConfig> o, List<ClusterConfig> i)
    {
        List<RouteConfig> a = new();
        List<ClusterConfig> b = new();
        int number = 0;
        DockerClient client = new DockerClientConfiguration().CreateClient();
        var containerinfo = client.Containers.ListContainersAsync(new ContainersListParameters()).Result;
        foreach (var c in containerinfo)
        {
            if (c.Ports.Count >= 1)
            {
                foreach (Port p in c.Ports)
                {
                    if (p.PublicPort == 80)
                    {
                        a.Add(new RouteConfig()
                        {
                            RouteId = number.ToString(),
                            ClusterId = number.ToString(),
                            Match = new RouteMatch
                            {
                                Hosts = new List<string>() { $"{c.Names[0].Replace("/", "")}.bigpod.si" }

                            }
                        });

                        b.Add(new ClusterConfig()
                        {
                            ClusterId = number.ToString(),
                            SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
                            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                            {
                                {
                                    "destination1", new DestinationConfig() { Address = $"http://{c.Names[0].Replace("/", "")}" }
                                },
                            }
                        });

                        number++;
                        Console.WriteLine(c.Names[0]);
                    }
                    else if(p.PublicPort == 443)
                    {
                        a.Add(new RouteConfig()
                        {
                            RouteId = number.ToString(),
                            ClusterId = number.ToString(),
                            Match = new RouteMatch
                            {
                                Hosts = new List<string>() { $"{c.Names[0].Replace("/", "")}.bigpod.si" }

                            }
                        });

                        b.Add(new ClusterConfig()
                        {
                            ClusterId = number.ToString(),
                            SessionAffinity = new SessionAffinityConfig { Enabled = true, Policy = "Cookie", AffinityKeyName = ".Yarp.ReverseProxy.Affinity" },
                            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                            {
                                {
                                    "destination1", new DestinationConfig() { Address = $"http://{c.Names[0].Replace("/", "")}" }
                                },
                            }
                        });

                        number++;
                        Console.WriteLine(c.Names[0]);
                    }
                }
            }
        }


        x.Update(a, b);

        foreach (var item in a)
            Console.WriteLine(item.ClusterId);
    }
}