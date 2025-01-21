using System.Collections.ObjectModel;
using MediatR;

namespace OutBoxPattern_POC.Entities;

public class Order : IHasDomainEvents
{
	public int Id { get; set; }
	public string ProductName { get; set; }
	public int Quantity { get; set; }

	private readonly List<INotification> _domainEvents = new();

	public IReadOnlyList<INotification> DomainEvents => _domainEvents;

	public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
	public void ClearDomainEvents() => _domainEvents.Clear();
}
