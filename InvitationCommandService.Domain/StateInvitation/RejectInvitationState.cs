using InvitationQueryService.Domain.Exceptions;

namespace InvitationQueryService.Domain.StateInvitation
{
    public class RejectInvitationState : IStateInvitation
    {
        public void Send() { }
        public void Accept() => throw new OperationException("You can't reject because the member exists in subscription.");
        public void Cancel() => throw new OperationException("You can't reject because the member don't exists in subscription.");
        public void Reject() => throw new OperationException("You can't reject because the member already reject.");
        public void Join() => throw new OperationException("You can't reject because the member exists in subscription.");
        public void ChangePermissions() => throw new OperationException("You can't reject because the member exists in subscription.");
        public void Leave() => throw new OperationException("You can't reject because the member don't exists in subscription.");
        public void Remove() => throw new OperationException("You can't reject because the member don't exists in subscription.");
        public void Empty() => throw new OperationException("You can't reject because the member don't exists in subscription.");
    }
}
