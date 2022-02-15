using Docker.DotNet.Models;
using Docker.DotNet;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Model;

namespace DockerRProxy;

static class update{
    public static InMemoryConfigProvider x;

    public static void runUpdate(List<RouteConfig> o, List<ClusterConfig> i)
    {
        List<RouteConfig> a = new();
        List<ClusterConfig> b = new();

        DockerClient client = new DockerClientConfiguration().CreateClient();
        var containerinfo = client.Containers.ListContainersAsync(new ContainersListParameters()).Result;
        foreach(var c in containerinfo)
        {
            Console.WriteLine(c.Names[0]);
        }


        x.Update(a,b);
    }
}