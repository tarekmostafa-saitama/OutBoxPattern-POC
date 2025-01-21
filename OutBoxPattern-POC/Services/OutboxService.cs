using MediatR;
using OutBoxPattern_POC.DataContext;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace OutBoxPattern_POC.Services;

public class OutboxService(AppDbContext dbContext, IMediator mediator) : IOutboxService
{
	public async Task ProcessOutboxAsync()
	{
		var messages = await dbContext.OutboxMessages.Where(x=>x.ProcessedOnUtc == null).ToListAsync();

		var tasks = new List<Task>();

		foreach (var message in messages)
		{
			try
			{
				// Deserialize the event
				var eventType = Type.GetType(message.Type);
				if (eventType == null) continue;

				if (JsonSerializer.Deserialize(message.Content, eventType) is INotification domainEvent)
				{
					tasks.Add(mediator.Publish(domainEvent));
				}

				// Mark as processed
				message.ProcessedOnUtc = DateTime.UtcNow;
			}
			catch (Exception ex)
			{
				// Save error details
				message.Error = ex.Message;
			}
		}

		await Task.WhenAll(tasks);
		await dbContext.SaveChangesAsync();
	}
}
