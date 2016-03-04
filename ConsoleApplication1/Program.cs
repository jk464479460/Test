using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Actors;
using Akka.Actor;
using Akka.Configuration;
using Message;

namespace ConsoleApplication1
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
            port = 8081
            hostname = localhost
        }
    }
}
");
            using (var system = ActorSystem.Create("MyServer", config))
            {
                system.ActorOf<ServerActor>("ServerActor");
                Console.ReadLine();
            }


        }
    }

    class ServerActor : TypedActor, IHandle<Request>
    {
        public void Handle(Request message)
        {
            var str = $"yes i have receive {message.Content},now i reply you ****!";
            Console.WriteLine(str);
            var response=new Response {Content="respon you "};
            Sender.Tell(response, Self);
            try
            {
                Thread.CurrentThread.Abort();
            }
            catch (Exception) { }
            
        }
    }
}
