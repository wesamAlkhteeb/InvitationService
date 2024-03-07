
using InvitationQueryService.Domain.Entities.Data;

namespace InvitationQueryService.Domain.Entities.Events
{
    public class CancelInvitationEventEntity : EventEntity<InvitationInfoData>
    {
        public override dynamic GetData() => this.Data;
    }

}
