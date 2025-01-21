using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using OutBoxPattern_POC.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OutBoxPattern_POC.DataContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Order> Orders { get; set; }
	public DbSet<OutboxMessage> OutboxMessages { get; set; }

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		// Collect domain events from tracked entities
		var domainEvents = ChangeTracker
			.Entries<IHasDomainEvents>()
			.SelectMany(e => e.Entity.DomainEvents)
			.ToList();

		// Clear domain events after collection
		foreach (var entity in ChangeTracker.Entries<IHasDomainEvents>())
		{
			entity.Entity.ClearDomainEvents();
		}

		// Persist entities and domain events in a single transaction
		var result = await base.SaveChangesAsync(cancellationToken);

		if (domainEvents.Any())
		{
			// Add domain events to outbox
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
			};

			var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage
			{
				Type = domainEvent.GetType().FullName,
				Content = JsonSerializer.Serialize((object)domainEvent, options), // Cast to object to ensure proper serialization
				OccurredOnUtc = DateTime.UtcNow
			}).ToList();

			OutboxMessages.AddRange(outboxMessages);

			await base.SaveChangesAsync(cancellationToken); // Save outbox messages
		}

		return result;
	}
}
