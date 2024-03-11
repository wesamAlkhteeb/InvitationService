

using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class RemoveInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
