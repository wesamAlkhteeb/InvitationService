

using InvitationQueryService.Domain.Entities.Data;

namespace InvitationQueryService.Domain.Entities.Events
{
    public class AcceptInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
