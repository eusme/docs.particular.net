using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClientUI;

public class InputLoopService(IMessageSession messageSession, ILogger<InputLoopService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            Console.Title = "ClientUI";

            logger.LogInformation("Press 'P' to place an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.P:
                    // Instantiate the command
                    var command = new PlaceOrder
                    {
                        OrderId = Guid.NewGuid().ToString()
                    };

                    // Send the command
                    logger.LogInformation("Sending PlaceOrder command, OrderId = {OrderId}", command.OrderId);
                    await messageSession.Send(command);

                    break;

                case ConsoleKey.Q:
                    return;

                default:
                    logger.LogInformation("Unknown input. Please try again.");
                    break;
            }
        }
    }
}
