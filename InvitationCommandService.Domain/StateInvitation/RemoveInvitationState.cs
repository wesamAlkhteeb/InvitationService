using InvitationCommandService.Domain.Exceptions;

namespace InvitationCommandService.Domain.StateInvitation
{
    public class RemoveInvitationState : IStateInvitation
    {
        public void Send() => throw new OperationException("You can't remove because you don't exists in subscription.");
        public void Accept() { }
        public void Cancel() => throw new OperationException("You can't remove because you don't exists in subscription.");
        public void Reject() => throw new OperationException("You can't remove because you don't exists in subscription.");
        public void Join() { }
        public void ChangePermissions() { }
        public void Leave() => throw new OperationException("You can't reomve because you don't exists in subscription.");
        public void Remove() => throw new OperationException("You can't remove because the member didn't exists in subscription.");

        public void Empty() => throw new OperationException("You can't remove because the member didn't exists in subscription.");
    }
}
