using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Database;
using InvitationCommandService.Domain.Entities;
using InvitationCommandService.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace InvitationCommandService.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly InvitationDbContext database;

        public EventRepository(InvitationDbContext database)
        {
            this.database = database;
        }
        public async Task<int> AddEvent(EventEntity eventEntity)
        {
            await database.Events.AddAsync(eventEntity);
            await database.Outboxes.AddAsync(new OutboxMessageEntity(eventEntity));
            await database.SaveChangesAsync();
            return eventEntity.Id;
        }

        public async Task<List<EventEntity>> GetEventsByAggregateId(string aggregateId)
        {
            return await database.Events
                    .Where(@event=> @event.AggregateId == aggregateId)
                    .OrderByDescending(@event=> @event.Sequence)
                    .ToListAsync();
        }

        public async Task<EventEntity?> GetLastEventByAggregateId(string aggregateId)
        {
            var a = await database.Events
                    .Where(@event => @event.AggregateId == aggregateId)
                    .OrderBy(@event => @event.Sequence).LastOrDefaultAsync();
            return a;

        }
    }
}
