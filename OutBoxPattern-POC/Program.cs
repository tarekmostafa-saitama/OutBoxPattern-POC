//TODO: Use Batch processing to process the outbox in bulk

using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OutBoxPattern_POC.DataContext;
using OutBoxPattern_POC.Entities;
using OutBoxPattern_POC.Services;

class Program
{
	static async Task Main(string[] args)
	{
		// Configure services
		var services = new ServiceCollection();
		services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
		services.AddScoped<IOutboxService, OutboxService>();
		services.AddScoped<OrderService>();

		// Build the service provider
		var serviceProvider = services.BuildServiceProvider();
		var orderService = serviceProvider.GetRequiredService<OrderService>();
		var outboxService = serviceProvider.GetRequiredService<IOutboxService>();

		// Simulate placing an order
		var order = new Order { Id = 1, ProductName = "Laptop", Quantity = 1 };

		await orderService.PlaceOrderAsync(order);

		// Process the outbox every 5 seconds
		while (true)
		{
			Console.WriteLine("\nProcessing Outbox...");
			await outboxService.ProcessOutboxAsync();
			await Task.Delay(5000);
		}
	}
}
