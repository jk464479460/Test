using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Message;

namespace Actors
{
    public class GreetingActor :ReceiveActor
    {
        public GreetingActor()
        {
            Receive<GreetingMessage>(greet =>
            {
                Console.WriteLine($"{greet.Content}");
                //Sender.Tell(new GreetingMessage {Content= "yes ,this is response"});
            }); //收到消息时执行
        }

    }

}
