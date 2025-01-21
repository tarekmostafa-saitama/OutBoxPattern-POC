using MediatR;

namespace OutBoxPattern_POC.Entities;

public interface IHasDomainEvents
{
	IReadOnlyList<INotification> DomainEvents { get; }

	void AddDomainEvent(INotification eventItem);
	void ClearDomainEvents();
}
