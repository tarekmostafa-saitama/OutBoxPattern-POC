using OutBoxPattern_POC.Entities;
using MediatR;


namespace OutBoxPattern_POC.Events;


public record OrderPlacedEvent(Order Order) : INotification;
