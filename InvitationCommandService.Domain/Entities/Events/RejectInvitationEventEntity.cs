
using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class RejectInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
