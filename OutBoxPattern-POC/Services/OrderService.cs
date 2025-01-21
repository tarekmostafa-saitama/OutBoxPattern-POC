using OutBoxPattern_POC.DataContext;
using OutBoxPattern_POC.Entities;
using OutBoxPattern_POC.Events;

namespace OutBoxPattern_POC.Services;

public class OrderService(AppDbContext dbContext)
{
	public async Task PlaceOrderAsync(Order order)
	{
		// Add order and attach domain event
		dbContext.Orders.Add(order);
		order.AddDomainEvent(new OrderPlacedEvent(order));

		// Save changes (will persist both the order and its domain event)
		await dbContext.SaveChangesAsync();
	}
}
