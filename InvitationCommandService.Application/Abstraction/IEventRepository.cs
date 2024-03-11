using InvitationCommandService.Domain.Entities.Events;

namespace InvitationCommandService.Application.Abstraction
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetLastEventByAggregateId(string aggregateId);
        Task<List<EventEntity>> GetEventsByAggregateId(string aggregateId);
        Task<int> AddEvent(EventEntity eventEntity);
    }
}
