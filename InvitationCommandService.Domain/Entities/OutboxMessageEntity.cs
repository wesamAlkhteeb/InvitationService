

using InvitationQueryService.Domain.Entities.Events;

namespace InvitationQueryService.Domain.Entities
{
    public class OutboxMessageEntity
    {
        private OutboxMessageEntity(int id)
        {
            Id = id;
        }

        public OutboxMessageEntity(EventEntity @event)
        {
            Event = @event;
        }

        public int Id { get; set; }
        public EventEntity? Event { get; set; }
    }
}
