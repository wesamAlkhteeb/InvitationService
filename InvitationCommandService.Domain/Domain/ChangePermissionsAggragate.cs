using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Exceptions;
using InvitationCommandService.Domain.Model;
using InvitationCommandService.Domain.StateInvitation;
using InvitationCommandService.Domain.Exceptions;

namespace InvitationCommandService.Domain.Domain
{
    public class ChangePermissionsAggragate : Aggregate
    {
        private List<PermissionsModel> Permissions;
        private ChangePermissionsAggragate(IStateInvitation stateInvitation, int subscribtionId, int memberId, List<PermissionsModel> permissions)
        {
            this.State = stateInvitation;
            GenerateAggregateId(subscribtionId, memberId);
            this.Permissions = permissions;
        }
        public static Aggregate GenerateAggregate(IStateInvitation stateInvitation, int subscribtionId, int memberId , List<PermissionsModel> permissions)
        {
            Aggregate aggregate = new ChangePermissionsAggragate(stateInvitation, subscribtionId, memberId,permissions);
            
            return aggregate;
        }

        public void HaveSamePermissions()
        {
            int index = Events!.Count() - 1;
            while (!EventHavePermission(index))
            {
                if (--index < 0) break;
            }
            if(index <0) {
                return;
            }
            InvitationData invitationData = Events![index].GetData();
            if (Permissions.Count != invitationData.Permissions.Count()) return;
            foreach (var permission in Permissions)
            {
                
                PermissionsModel? permissionsModel = invitationData.Permissions.Find(x => x.Id == permission.Id);
                if (permissionsModel == null) return;
            }
            throw new RepeatPermissionException("You tried to set existing permissions.");
        }

        private bool EventHavePermission(int index)
        {
            string type = Events![index].Type!;
            return type == EventType.SendEvent.ToString() || 
                type == EventType.JoinEvent.ToString() || 
                type == EventType.ChangePermissionEvent.ToString();
        }

        public override void CanDoEvent()
        {
            Check();
            HaveSamePermissions();
        }
    }
}
