
using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class SendInvitationEventEntity : EventEntity<InvitationData>
    {
        public override dynamic GetData() => this.Data;
    }

}
