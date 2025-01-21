namespace OutBoxPattern_POC.Events;

using MediatR;
using OutBoxPattern_POC.Entities;
using OutBoxPattern_POC.Services;
using System.Text.Json;

public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
{
	private readonly IOutboxService _outboxService;

	public OrderPlacedEventHandler(IOutboxService outboxService)
	{
		_outboxService = outboxService;
	}

	public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
	{
		if (notification?.Order == null)
			throw new ArgumentNullException(nameof(notification));

		Console.WriteLine($"Notification: New order has been placed with product name= {notification.Order.ProductName}");
	}
}
