using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Event;
using Message;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            applied-adapters = []
            transport-protocol = tcp
            port = 0
            hostname = localhost
        }
    }
}
");

            using (var system = ActorSystem.Create("MyClient", config))
            {
                //while (true)
                {
                    var s = Console.ReadLine();
                    var client =system.ActorOf(Props.Create<ClientActor>());
                    system.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ServerActor");
                    client.Tell(new Request { Content = "天啊吗" });
                    Thread.Sleep(10000);
                    //system.Stop(client);
                    system.Terminate();
                    system.Stop(client);
                }
               
            }
            
        }
      
    }

    class ClientActor : TypedActor, IHandle<Response>,IHandle<Request>
    {
        private readonly ActorSelection _server = Context.ActorSelection("akka.tcp://MyServer@localhost:8081/user/ServerActor");

        public void Handle(Request message)
        {
            message.Content = "这个是我的请求";
            
            _server.Tell(message);
        }

        public void Handle(Response message)
        {
            Console.WriteLine(message.Content);
        }
    }
}
