

using InvitationQueryService.Domain.Entities.Data;

namespace InvitationQueryService.Domain.Entities.Events
{
    public class RemoveInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
