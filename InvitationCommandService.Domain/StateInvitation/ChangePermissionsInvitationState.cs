using InvitationQueryService.Domain.Exceptions;

namespace InvitationQueryService.Domain.StateInvitation
{
    public class ChangePermissionsInvitationState : IStateInvitation
    {
        public void Send() => throw new OperationException("You can't change permissions because you must to accept or reject subscription before do anything.");
        public void Accept() { }
        public void Cancel() => throw new OperationException("You can't change permissions because you don't exists in subscription.");
        public void Reject() => throw new OperationException("You can't change permissions because you don't exists in subscription.");
        public void Join() { }
        public void ChangePermissions() { }
        public void Leave() => throw new OperationException("You can't change permissions because you don't exists in subscription.");
        public void Remove() => throw new OperationException("You can't change permissions because you don't exists in subscription.");

        public void Empty() => throw new OperationException("You can't change permissions because you don't exists in subscription.");
    }
}
