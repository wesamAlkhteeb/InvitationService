using InvitationCommandService.Domain.Exceptions;

namespace InvitationCommandService.Domain.StateInvitation
{
    public class JoinInvitationState : IStateInvitation
    {
        public void Send() { }
        public void Accept() => throw new OperationException("You can't join because you already exists in subscription.");
        public void Cancel() { }
        public void Reject() { }
        public void Join() => throw new OperationException("You can't join because the member already exists in subscription.");
        public void ChangePermissions() => throw new OperationException("You can't join because you exists in subscription.");
        public void Leave() { }
        public void Remove() { }

        public void Empty() { }
    }
}
