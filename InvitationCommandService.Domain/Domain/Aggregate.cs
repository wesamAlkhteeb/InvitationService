using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Domain.StateInvitation;

namespace InvitationCommandService.Domain.Domain
{
    public abstract class Aggregate
    {
        public string AggregateId { get; set; } = string.Empty;
        public int Sequence { get; set; } = 0;
        public int NextSequence => Sequence + 1;
        public List<EventEntity>? Events { get; private set; }
        public IStateInvitation? State { get; set; }
        
        public void loadEvents(List<EventEntity> events)
        {
            Events = events;
            if (events.Count() == 0)
                this.Sequence = 0;
            else
                this.Sequence = Events.Last().Sequence;
        }

        public void loadEvent(EventEntity? @event)
        {
            if (this.Events == null)
            {
                this.Events = new();
            }
            if (@event is not null)
            {
                this.Events.Add(@event);
                this.Sequence = @event.Sequence;
            }
        }

        public void GenerateAggregateId(int subscribtionId, int memberId)
        {
            this.AggregateId = $"{memberId}-{subscribtionId}";

        }
        
        protected void Check()
        {
            if (Events == null || State == null)
            {
                throw new Exception("Exception in Aggregate => CanDoEvent");
            }
            if (Events.Count == 0)
            {
                State.Empty();
            }
            else
            {
                switch (Events.Last().Type)
                {
                    case "SendEvent":
                        State.Send();
                        break;
                    case "CancelEvent":
                        State.Cancel();
                        break;
                    case "AcceptEvent":
                        State.Accept();
                        break;
                    case "RejectEvent":
                        State.Reject();
                        break;
                    case "JoinEvent":
                        State.Join();
                        break;
                    case "RemoveEvent":
                        State.Remove();
                        break;
                    case "ChangePermissionEvent":
                        State.ChangePermissions();
                        break;
                    case "LeaveEvent":
                        State.Leave();
                        break;
                    default: throw new Exception("Exception in Aggregate => CanDoEvent");
                }
            }
        }

        public abstract void CanDoEvent();
        
    }

}

/*
 
 builder.HasDiscriminator(x => x.Type)
                .HasValue<JoinInvitationEventEntity>("JoinEvent")
                .HasValue<RemoveInvitationEventEntity>("RemoveEvent")
                .HasValue<ChangePermissionsInvitationEventEntity>("ChangePermissionEvent")
                .HasValue<LeaveInvitationEventEntity>("LeaveEvent");
 */