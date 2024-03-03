

using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class AcceptInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
