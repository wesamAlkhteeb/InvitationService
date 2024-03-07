using InvitationQueryService.Domain.Entities.Events;

namespace InvitationQueryService.Application.Abstraction
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetLastEventByAggregateId(string aggregateId);
        Task<List<EventEntity>> GetEventsByAggregateId(string aggregateId);
        Task<int> AddEvent(EventEntity eventEntity);
    }
}
