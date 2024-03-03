using InvitationCommandService.Domain.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.Abstraction
{
    public interface IEventRepository
    {
        Task<EventEntity?> GetLastEventByAggregateId(string aggregateId);
        Task<List<EventEntity>> GetEventsByAggregateId(string aggregateId);
        Task<int> AddEvent(EventEntity eventEntity);
    }
}
