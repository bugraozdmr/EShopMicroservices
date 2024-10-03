using MediatR;

namespace Ordering.Domain.Abstractions;

// domaine buildingblocks'dan bağlantı yapmadık istemiyoruz çünkü -- direkt kendimiz yükledik mediatr'ı
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}