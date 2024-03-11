

namespace InvitationCommandService.Domain.Entities.Events
{

    public abstract class EventEntity
    {
        public int Id { get; set; }
        public required string AggregateId { get; set; }
        public required int Sequence { get; set; }
        public required DateTime DateTime { get; set; }
        public abstract dynamic GetData();
        public string? Type { get; set; }
    }

    public abstract class EventEntity<T> : EventEntity
    {
        public required T Data { get; set; }
    }
}
