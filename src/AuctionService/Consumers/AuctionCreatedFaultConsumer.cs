using System;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
       Console.WriteLine(" --> Consuming faulty cration of auction");
       
       var excecption = context.Message.Exceptions.First();

       if(excecption.ExceptionType == "System.ArgumentException")
       {
            context.Message.Message.Model = "FooBar";
            await context.Publish(context.Message.Message);

       }
        else
        {
            Console.WriteLine(" --> Auction creation failed for an unknown reason");
        }
    }
}
