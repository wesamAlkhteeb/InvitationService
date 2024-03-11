
using InvitationCommandService.Domain.Entities.Data;

namespace InvitationCommandService.Domain.Entities.Events
{
    public class ChangePermissionsInvitationEventEntity : EventEntity<InvitationData>
    {
        public override dynamic GetData() => this.Data;
    }

}
