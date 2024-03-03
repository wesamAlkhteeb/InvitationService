
using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class LeaveInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
