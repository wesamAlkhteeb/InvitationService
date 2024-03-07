
using InvitationQueryService.Domain.Entities.Data;

namespace InvitationQueryService.Domain.Entities.Events
{
    public class JoinInvitationEventEntity : EventEntity<InvitationData>
    {
        public override dynamic GetData() => this.Data;
    }

}
