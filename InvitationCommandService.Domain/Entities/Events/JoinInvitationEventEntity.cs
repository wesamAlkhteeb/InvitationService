
using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class JoinInvitationEventEntity : EventEntity<InvitationData>
    {
        public override dynamic GetData() => this.Data;
    }

}
