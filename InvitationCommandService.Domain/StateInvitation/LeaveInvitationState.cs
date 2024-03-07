using InvitationQueryService.Domain.Exceptions;

namespace InvitationQueryService.Domain.StateInvitation
{
    public class LeaveInvitationState : IStateInvitation
    {
        public void Send() => throw new OperationException("You can't leave because you don't exists in subscription.");
        public void Accept() { }
        public void Cancel() => throw new OperationException("You can't leave because you don't exists in subscription.");
        public void Reject() => throw new OperationException("You can't leave because you don't exists in subscription.");
        public void Join() { }
        public void ChangePermissions() { }
        public void Leave() => throw new OperationException("You can't leave because you don't exists in subscription.");
        public void Remove() => throw new OperationException("You can't leave because the member didn't exists in subscription.");

        public void Empty() => throw new OperationException("You can't leave because the member didn't exists in subscription.");
    }
}
